using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    //Allows to read array from json file
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
