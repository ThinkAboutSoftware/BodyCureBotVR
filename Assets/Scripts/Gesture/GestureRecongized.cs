using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class GestureRecongized : MonoBehaviour
{
    float speed = 5.0f;

    private OVRSkeleton leftSkeleton;
    private OVRSkeleton rightSkeleton;
    private List<OVRBone> leftBones;
    private List<OVRBone> rightBones;
    public List<Gesture> saveGestures;

    public bool startChecked = true;
    public GameObject leftHand;
    public GameObject rightHand;
    private Gesture preGesture;
    private Gesture currentGesture;

    public GameObject ovrCamera;
    public GameObject centerCam;


    private Vector3 centerCamDir;

    void Start()
    {
        LoadData();
        leftSkeleton = leftHand.GetComponent<OVRSkeleton>();
        rightSkeleton = rightHand.GetComponent<OVRSkeleton>();
        preGesture = new Gesture("preGesture");
    }

    void Update()
    {   
        if (startChecked)
        {
            leftBones = new List<OVRBone>(leftSkeleton.Bones);
            rightBones = new List<OVRBone>(rightSkeleton.Bones);
            currentGesture = Recognized();

            if (!currentGesture.name.Equals("checked"))
            {
                Debug.Log("new Gesture: " + currentGesture.name);

                if(currentGesture.name.Equals("Left_go_v1")|| currentGesture.name.Equals("Left_go_v2"))
                {

                    Vector3 centerCamDir = centerCam.transform.forward;
                    centerCamDir.y = 0;
                    centerCamDir.Normalize();

                    ovrCamera.transform.Translate(centerCamDir * speed * Time.deltaTime);

                }

                if(currentGesture.name.Equals("Right_back_v1")|| currentGesture.name.Equals("Right_back_v2"))
                {

                    Vector3 centerCamDir = -centerCam.transform.forward;
                    centerCamDir.y = 0;
                    centerCamDir.Normalize();

                    ovrCamera.transform.Translate(centerCamDir * speed * Time.deltaTime);

                }

                preGesture = currentGesture;
                currentGesture.onRecognized.Invoke();
            }
        }
    }

    public void LoadData()
    {
        string jsondata = File.ReadAllText(Application.dataPath + "/Scripts/Gesture/data.json");
        var datas = JsonHelper.FromJson<Data>(jsondata);


        for (int i = 0; i < datas.Length; i++)
        {
            Gesture addGesture = new Gesture(datas[i].gesturename);
            List<Vector3> checkedPosition = new List<Vector3>();
            for (int j = 0; j < datas[i].bonepositions.Length; j++)
            {
                checkedPosition.Add(datas[i].bonepositions[j]);
            }
            addGesture.SetPosition(checkedPosition);
            saveGestures.Add(addGesture);
        }
    }

    private Gesture Recognized()
    {

        float checkedDist = 0.05f;
        float minDist = Mathf.Infinity;
        Gesture checkedGesture = new Gesture("checked");

        foreach (var saveGesture in saveGestures)
        {
            bool discardGesture = false;
            float sumDist = 0.0f;
            for (int i = 2; i < leftBones.Count; i++)
            {
                Vector3 pos = leftHand.transform.InverseTransformPoint(leftBones[i].Transform.position);
                float distance = Vector3.Distance(saveGesture.positions[i], pos);
 
                if (distance > checkedDist)
                {
                    discardGesture = true;
                    break;
                }
                sumDist += distance;
            }

            if (!discardGesture && sumDist < minDist)
            {
                checkedGesture = saveGesture;
                minDist = sumDist;
            }
        }
        foreach (var saveGesture in saveGestures)
        {
            bool discardGesture = false;
            float sumDist = 0.0f;
            for (int i = 2; i < rightBones.Count; i++)
            {
                Vector3 pos = rightHand.transform.InverseTransformPoint(rightBones[i].Transform.position);
                float distance = Vector3.Distance(saveGesture.positions[i], pos);

                if (distance > checkedDist)
                {
                    discardGesture = true;
                    break;
                }
                sumDist += distance;
            }

            if (!discardGesture && sumDist < minDist)
            {
                checkedGesture = saveGesture;
                minDist = sumDist;
            }
        }
        return checkedGesture;
    }
}