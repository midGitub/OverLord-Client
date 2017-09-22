

using SimpleJson;
using System;
using System.Collections.Generic;
using UnityEngine;


public class TableReader
{
    public static Dictionary<int, JsonObject> ReadTable(string filepath)
    {
        object obj;
        SimpleJson.SimpleJson.TryDeserializeObject(((TextAsset)Resources.Load(filepath)).text, out obj);
        List<object> list1 = (List<object>)obj;
        List<object> list2 = (List<object>)list1[0];
        int count1 = list2.Count;
        List<string> list3 = new List<string>();
        for (int index = 0; index < count1; ++index)
            list3.Add(Convert.ToString(list2[index]));
        List<object> list4 = (List<object>)list1[2];
        List<string> list5 = new List<string>();
        for (int index = 0; index < count1; ++index)
            list5.Add(Convert.ToString(list4[index]));
        list1.RemoveAt(0);
        list1.RemoveAt(0);
        list1.RemoveAt(0);
        int count2 = list1.Count;
        Dictionary<int, JsonObject> dictionary = new Dictionary<int, JsonObject>();
        for (int index1 = 0; index1 < count2; ++index1)
        {
            List<object> list6 = (List<object>)list1[index1];
            int key = Convert.ToInt32(list6[0]);
            JsonObject jsonObject = new JsonObject();
            for (int index2 = 0; index2 < count1; ++index2)
            {
                if (list3[index2] == "NUM")
                    jsonObject[list5[index2]] = (object)Convert.ToInt32(list6[index2]);
                else if (list3[index2] == "FLOAT")
                    jsonObject[list5[index2]] = (object)(float)Convert.ToDouble(list6[index2]);
                else if (list3[index2] == "TEXT")
                    jsonObject[list5[index2]] = (object)Convert.ToString(list6[index2]);
                else
                    Debug.LogError((object)("表格" + filepath + "填写有误"));
            }
            dictionary.Add(key, jsonObject);
        }
        return dictionary;
    }
}

