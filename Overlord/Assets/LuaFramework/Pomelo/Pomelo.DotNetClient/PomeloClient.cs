// Decompiled with JetBrains decompiler
// Type: Pomelo.DotNetClient.PomeloClient
// Assembly: LuaFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2AE44236-98FA-40BB-AED3-FFBE9AFD40C3
// Assembly location: D:\ProjectsFiles\UnityProjects\seaWarClient\Assets\3rdPlugins\LuaFramework.dll

using SimpleJson;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Pomelo.DotNetClient
{
    public class PomeloClient : IDisposable
    {
        private uint reqId = 1U;
        private ManualResetEvent timeoutEvent = new ManualResetEvent(false);
        private int timeoutMSec = 8000;
        private JsonObject emptyMsg = new JsonObject();
        public NWorkStateChangedEvent NetWorkStateChangedEvent;
        private NetWorkState netWorkState;
        private EventManager eventManager;
        private Socket socket;
        private Protocol protocol;
        private bool disposed;

        public void initClient(string host, int port, Action callback = null)
        {
            this.timeoutEvent.Reset();
            this.eventManager = new EventManager();
            this.NetWorkChanged(NetWorkState.CONNECTING);
            IPAddress address = (IPAddress)null;
            string newServerIp = "";
            AddressFamily mIPType = AddressFamily.InterNetwork;
            IPv6SupportMidleware.getIPType(host, port.ToString(), out newServerIp, out mIPType);
            if (!string.IsNullOrEmpty(newServerIp))
                host = newServerIp;
            this.socket = new Socket(mIPType, SocketType.Stream, ProtocolType.Tcp);
            if (!IPAddress.TryParse(host, out address))
                Debug.LogWarning((object)"**********8IP地址格式错误***********");
            if (address == null)
                throw new Exception("can not parse host : " + host);
            this.socket.BeginConnect((EndPoint)new IPEndPoint(address, port), (AsyncCallback)(result =>
            {
                try
                {
                    this.socket.EndConnect(result);
                    this.protocol = new Protocol(this, this.socket);
                    this.NetWorkChanged(NetWorkState.CONNECTED);
                    if (callback == null)
                        return;
                    callback();
                }
                catch (SocketException ex)
                {
                    Console.WriteLine((object)ex);
                    if (this.netWorkState != NetWorkState.TIMEOUT)
                        this.NetWorkChanged(NetWorkState.ERROR);
                    this.Dispose();
                }
                finally
                {
                    this.timeoutEvent.Set();
                }
            }), (object)this.socket);
            if (!this.timeoutEvent.WaitOne(this.timeoutMSec, false) || this.netWorkState == NetWorkState.CONNECTED || this.netWorkState == NetWorkState.ERROR)
                return;
            this.NetWorkChanged(NetWorkState.TIMEOUT);
            this.Dispose();
        }

        private void NetWorkChanged(NetWorkState state)
        {
            this.netWorkState = state;
            if (this.NetWorkStateChangedEvent == null)
                return;
            this.NetWorkStateChangedEvent(state);
        }

        public void connect()
        {
            this.connect((JsonObject)null, (Action<JsonObject>)null);
        }

        public void connect(JsonObject user)
        {
            this.connect(user, (Action<JsonObject>)null);
        }

        public void connect(Action<JsonObject> handshakeCallback)
        {
            this.connect((JsonObject)null, handshakeCallback);
        }

        public bool connect(JsonObject user, Action<JsonObject> handshakeCallback)
        {
            try
            {
                this.protocol.start(user, handshakeCallback);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log((object)ex.ToString());
                return false;
            }
        }

        public void request(string route, Action<JsonObject> action)
        {
            this.request(route, this.emptyMsg, action);
        }

        public void request(string route, JsonObject msg, Action<JsonObject> action)
        {
            this.eventManager.AddCallBack(this.reqId, action);
            this.protocol.send(route, this.reqId, msg);
            ++this.reqId;
        }

        public void notify(string route, JsonObject msg)
        {
            this.protocol.send(route, msg);
        }

        public void on(string eventName, Action<JsonObject> action)
        {
            this.eventManager.AddOnEvent(eventName, action);
        }

        internal void processMessage(Message msg)
        {
            if (msg.type == MessageType.MSG_RESPONSE)
            {
                this.eventManager.InvokeCallBack(msg.id, msg.data);
            }
            else
            {
                if (msg.type != MessageType.MSG_PUSH)
                    return;
                this.eventManager.InvokeOnEvent(msg.route, msg.data);
            }
        }

        public void disconnect()
        {
            this.Dispose();
            this.NetWorkChanged(NetWorkState.DISCONNECTED);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed || !disposing)
                return;
            if (this.protocol != null)
                this.protocol.close();
            if (this.eventManager != null)
                this.eventManager.Dispose();
            try
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
                this.socket = (Socket)null;
            }
            catch (Exception ex)
            {
            }
            this.disposed = true;
        }
    }
}
