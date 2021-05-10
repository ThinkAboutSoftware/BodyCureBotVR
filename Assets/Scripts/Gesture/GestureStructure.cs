using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

[System.Serializable]
public class Gesture
{
    public string name;
    public List<Vector3> positions;
    public UnityEvent onRecognized;

    public Gesture(string _name)
    {
        name = _name;
        positions = new List<Vector3>();
        onRecognized = new UnityEvent();
    }

    public void SetPosition(List<Vector3> _positions)
    {
        for(int i=0;i< _positions.Count; i++)
        {
            positions.Add(_positions[i]);
        }
    }
}

[System.Serializable]
public class Data
{
    public string gesturename;
    public Vector3[] bonepositions;

    public Data()
    {
        bonepositions = new Vector3[24];
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}

public class GestureStructure : MonoBehaviour
{

    

}