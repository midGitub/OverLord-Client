
using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpEntry
{
    public static IEnumerator Send(string url, WWWForm form, Action<int, JsonObject> GetValueBackEvent)
    {
        WWW www = new WWW(url, form);
        yield return (object)www;
        if (www.isDone)
        {
            object obj;
            SimpleJson.SimpleJson.TryDeserializeObject(www.text, out obj);
            GetValueBackEvent(0, (JsonObject)(obj as JsonObject)["body"]);
        }
    }

    public static IEnumerator LSend(string url, WWWForm form)
    {
        WWW www = new WWW(url, form);
        yield return (object)www;
        if (www.isDone)
        {
            object obj = null;
            SimpleJson.SimpleJson.TryDeserializeObject(www.text, out obj);
            Debug.Log("-------------------------");
            JsonObject jsonObject = (JsonObject)(obj as JsonObject)["body"];
            Debug.Log("-------------------------" + jsonObject.ToString());
            PomeloSocketEntry.sRcvedMsgEvents.Enqueue(new KeyValuePair<int, string>(Convert.ToInt32(jsonObject["id"]), jsonObject.ToString()));
            Debug.Log(Convert.ToInt32(jsonObject["id"]) + jsonObject.ToString());
        }
    }
}
