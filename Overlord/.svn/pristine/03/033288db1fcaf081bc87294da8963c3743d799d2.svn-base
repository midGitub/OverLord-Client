// Decompiled with JetBrains decompiler
// Type: PomeloSocketEntry
// Assembly: LuaFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2AE44236-98FA-40BB-AED3-FFBE9AFD40C3
// Assembly location: D:\ProjectsFiles\UnityProjects\seaWarClient\Assets\3rdPlugins\LuaFramework.dll


using Pomelo.DotNetClient;
using SimpleJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PomeloSocketEntry
{
    private static PomeloClient Client = (PomeloClient)null;
    public static NetWorkState networkState = NetWorkState.CLOSED;
    public static Queue<KeyValuePair<int, string>> sRcvedMsgEvents = new Queue<KeyValuePair<int, string>>();
    public static GeneralDelegate<int> NwStateChangeDel;

    public static void Connect(string url, int port, string token, string route, Action<JsonObject> callback)
    {
        PomeloSocketEntry.Client = new PomeloClient();
        PomeloSocketEntry.Client.NetWorkStateChangedEvent += (NWorkStateChangedEvent)(state =>
        {
            PomeloSocketEntry.networkState = state;
            //Debug.Log((object)PomeloSocketEntry.networkState);
            if (PomeloSocketEntry.NwStateChangeDel == null)
                return;
            PomeloSocketEntry.NwStateChangeDel((int)PomeloSocketEntry.networkState);
        });
        PomeloSocketEntry.Client.initClient(url, port, (Action)(() =>
        {
            JsonObject user = new JsonObject();

            PomeloSocketEntry.Client.connect(user, (Action<JsonObject>)(responseData =>
            {
                Debug.Log(responseData +"handshake call back data");
                if (NetWorkState.CONNECTED != PomeloSocketEntry.networkState)
                    return;
                PomeloSocketEntry.SocketRequest(new JsonObject()
        {
          {
            "token",
            (object) token
          }
        }, route, callback);
            }));
        }));
    }

    public static void LConnect(string url, int port, string token, string route)
    {
        PomeloSocketEntry.Client = new PomeloClient();
        PomeloSocketEntry.Client.NetWorkStateChangedEvent += (NWorkStateChangedEvent)(state =>
        {
            PomeloSocketEntry.networkState = state;
            Debug.Log((object)PomeloSocketEntry.networkState);
            if (PomeloSocketEntry.NwStateChangeDel == null)
                return;
            PomeloSocketEntry.NwStateChangeDel((int)PomeloSocketEntry.networkState);
        });
        PomeloSocketEntry.Client.initClient(url, port, (Action)(() =>
        {
            JsonObject user = new JsonObject();
            PomeloSocketEntry.Client.connect(user, (Action<JsonObject>)(responseData =>
            {
                Debug.Log((object)(responseData.ToString() + (object)"handshake call back data"));
                if (NetWorkState.CONNECTED != PomeloSocketEntry.networkState)
                    return;
                PomeloSocketEntry.LSocketRequest(new JsonObject()
        {
          {
            "token",
            (object) token
          }
        }, route);
            }));
        }));
    }

    public static void LReconnect(string url, int port, int playerid, string route)
    {
        PomeloSocketEntry.Client = new PomeloClient();
        PomeloSocketEntry.Client.NetWorkStateChangedEvent += (NWorkStateChangedEvent)(state =>
        {
            PomeloSocketEntry.networkState = state;
            Debug.Log(PomeloSocketEntry.networkState);
            if (PomeloSocketEntry.NwStateChangeDel == null)
                return;
            PomeloSocketEntry.NwStateChangeDel((int)PomeloSocketEntry.networkState);
        });
        PomeloSocketEntry.Client.initClient(url, port, (Action)(() =>
        {
            JsonObject user = new JsonObject();
            PomeloSocketEntry.Client.connect(user, (Action<JsonObject>)(responseData =>
            {
                Debug.Log(responseData +"handshake call back data");
                if (NetWorkState.CONNECTED != PomeloSocketEntry.networkState)
                    return;
                PomeloSocketEntry.LSocketRequest(new JsonObject()
        {
          {
            "playerId",
            (object) playerid
          }
        }, route);
            }));
        }));
    }

    public static void SocketRequest(JsonObject sendData, string Route, Action<JsonObject> callback)
    {
        if (PomeloSocketEntry.Client != null)
        {
            object code;
            object err;
            PomeloSocketEntry.Client.request(Route, sendData, (Action<JsonObject>)(responseData =>
            {
                if (!object.ReferenceEquals((object)responseData, (object)null))
                {
                    Debug.Log((object)responseData);
                    responseData.TryGetValue("code", out code);
                    if (Convert.ToInt32(code).Equals(200))
                    {
                        callback(responseData);
                    }
                    else
                    {
                        responseData.TryGetValue("err", out err);
                        Debug.LogError((err.ToString() + "  " + Route));
                    }
                }
                else
                    Debug.Log("服务器未能返回数据，网络可能异常");
            }));
        }
        else
            Debug.Log((object)"------pomeloclient is null------");
    }

    public static void LSocketRequest(JsonObject sendData, string Route)
    {
        if (PomeloSocketEntry.Client != null)
            PomeloSocketEntry.Client.request(Route, sendData, (Action<JsonObject>)(responseData =>
            {
                if (responseData != null)
                    PomeloSocketEntry.sRcvedMsgEvents.Enqueue(new KeyValuePair<int, string>(Convert.ToInt32(responseData["id"]), responseData.ToString()));
                else
                    Debug.Log("服务器未能返回数据，网络可能异常");
            }));
        else
            Debug.Log("------pomeloclient is null------");
    }

    public static void On(string route, Action<JsonObject> callback)
    {
        if (PomeloSocketEntry.Client != null)
            PomeloSocketEntry.Client.on(route, callback);
        else
            Debug.Log("------pomeloclient is null------");
    }

    public static void LOn(string route)
    {
        if (PomeloSocketEntry.Client != null)
            PomeloSocketEntry.Client.on(route, (Action<JsonObject>)(responseData =>
            {
                if (responseData !=null)
                    PomeloSocketEntry.sRcvedMsgEvents.Enqueue(new KeyValuePair<int, string>(Convert.ToInt32(responseData["id"]), responseData.ToString()));
                else
                    Debug.Log("服务器未能返回数据，网络可能异常");
            }));
        else
            Debug.Log("------pomeloclient is null------");
    }

    public static void Notify(JsonObject data, string route)
    {
        if (PomeloSocketEntry.Client != null)
            PomeloSocketEntry.Client.notify(route, data);
        else
            Debug.Log("------pomeloclient is null------");
    }

    public static void DisConnect()
    {
        if (PomeloSocketEntry.Client == null)
            return;
        PomeloSocketEntry.Client.disconnect();
    }

    public static void Destroy()
    {
        PomeloSocketEntry.Client = (PomeloClient)null;
    }
}
