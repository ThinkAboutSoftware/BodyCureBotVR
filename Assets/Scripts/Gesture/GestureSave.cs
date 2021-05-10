using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class GestureSave : MonoBehaviour
{
    private OVRSkeleton leftSkeleton;
    private OVRSkeleton rightSkeleton;
    private List<OVRBone> leftBones;
    private List<OVRBone> rightBones;
    public List<Gesture> saveGestures;
    public bool leftDebugs = true;
    public bool rightDebugs = false;

    public GameObject leftHand;
    public GameObject rightHand;

    void Start()
    {
        leftSkeleton = leftHand.GetComponent<OVRSkeleton>();
        rightSkeleton = rightHand.GetComponent<OVRSkeleton>();
    }


    void Update()
    {
        if (leftDebugs && Input.GetKeyDown(KeyCode.Space))
        {
            leftBones = new List<OVRBone>(leftSkeleton.Bones);
            Save();
        }

        if (rightDebugs && Input.GetKeyDown(KeyCode.Space))
        {
            rightBones = new List<OVRBone>(rightSkeleton.Bones);
            Save();
        }
    }

    private void Save()
    {
        Gesture newgesture;
        newgesture = new Gesture("New Gesture");
        List<Vector3> pos = new List<Vector3>();

        if (!leftDebugs && !rightDebugs)
            return;

        if (leftDebugs)
        {
            foreach (var bone in leftBones)
            {
                pos.Add(leftHand.transform.InverseTransformPoint(bone.Transform.position));
            }
        }

        if (rightDebugs)
        {
            foreach (var bone in rightBones)
            {
                pos.Add(rightHand.transform.InverseTransformPoint(bone.Transform.position));
            }
        }
        
        newgesture.SetPosition(pos);
        saveGestures.Add(newgesture);
        Debug.Log("success saved!!");
    }

    public void SaveData()
    {
        Data[] data = new Data[saveGestures.Count];

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new Data();
            data[i].gesturename = saveGestures[i].name;

            for (int j = 0; j < data[i].bonepositions.Length; j++)
            {
                data[i].bonepositions[j] = new Vector3();
                data[i].bonepositions[j] = saveGestures[i].positions[j];
            }
        }

        string toJson = JsonHelper.ToJson(data, prettyPrint: true);
        File.WriteAllText(Application.dataPath + "/Scenes/script/data.json", toJson);

    }
}

