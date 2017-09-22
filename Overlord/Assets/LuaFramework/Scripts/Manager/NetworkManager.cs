
using SimpleJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuaFramework
{
    public class NetworkManager : Manager
    {
        private Queue<JsonObject> msgQuene = new Queue<JsonObject>();
        private Queue<Action<JsonObject>> handleMsgquene = new Queue<Action<JsonObject>>();

        public void OnInit()
        {
            this.CallMethod("Start");
        }

        public void Unload()
        {
            this.CallMethod("Unload");
        }

        public object[] CallMethod(string func, params object[] args)
        {
            return Util.CallMethod("Network", func, args);
        }

        private void Start()
        {
            PomeloSocketEntry.NwStateChangeDel += new GeneralDelegate<int>(this.NetworkStateChangeCallBack);
        }

        private void Update()
        {
            while (this.handleMsgquene.Count > 0)
                this.handleMsgquene.Dequeue()(this.msgQuene.Dequeue());
            this.msgQuene.Clear();
            this.handleMsgquene.Clear();
            if (PomeloSocketEntry.sRcvedMsgEvents.Count <= 0)
                return;
            while (PomeloSocketEntry.sRcvedMsgEvents.Count > 0)
                this.facade.SendMessageCommand("DispatchMessage", (object)PomeloSocketEntry.sRcvedMsgEvents.Dequeue());
        }

        public static void AddEvent(int _event, string data)
        {
            PomeloSocketEntry.sRcvedMsgEvents.Enqueue(new KeyValuePair<int, string>(_event, data));
        }

        public void ConnectSocket(string url, int port, string token, string route)
        {
            PomeloSocketEntry.LConnect(url, port, token, route);
        }

        public void ReconnectSocket(string url, int port, int playerid, string route)
        {
            PomeloSocketEntry.LReconnect(url, port, playerid, route);
        }

        public void SocketRequest(string sendData, string route)
        {
            object obj;
            SimpleJson.SimpleJson.TryDeserializeObject(sendData, out obj);
            Debug.Log((object)("SocketRequest Send Message" + obj));
            PomeloSocketEntry.LSocketRequest((JsonObject)obj, route);
        }

        public void ConnectHttp(string route, int id, string username, string pwd, int operate)
        {
            try
            {
                WWWForm form = new WWWForm();
                form.AddField("id", id);
                form.AddField("userId", username);
                form.AddField("password", pwd);
                form.AddField("operate", operate);
                Debug.Log(("登录服务器地址：http://" + AppConst.SocketAddress + ":" + AppConst.SocketPort + "/" + route));
                this.StartCoroutine(HttpEntry.LSend("http://" + AppConst.SocketAddress + ":" + AppConst.SocketPort + "/" + route, form));
            }
            catch (Exception ex)
            {
                Debug.LogError("输入错误");
            }
        }

        public void Listen(string route)
        {
            Debug.Log((object)("开始监听 " + route));
            PomeloSocketEntry.LOn(route);
        }

        public void DisConnectSocket()
        {
            PomeloSocketEntry.DisConnect();
        }

        private void NetworkStateChangeCallBack(int nws)
        {
            this.CallMethod("NetworkStateChangeCallBack", (object)nws);
        }
    }
}



//----------------------------------------原版LuaFramework.NetworkManager------------start---------------------------------------------------------------------

//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using LuaInterface;

//namespace LuaFramework {
//    public class NetworkManager : Manager {
//        private SocketClient socket;
//        static readonly object m_lockObject = new object();
//        static Queue<KeyValuePair<int, ByteBuffer>> mEvents = new Queue<KeyValuePair<int, ByteBuffer>>();

//        SocketClient SocketClient {
//            get { 
//                if (socket == null)
//                    socket = new SocketClient();
//                return socket;                    
//            }
//        }

//        void Awake() {
//            Init();
//        }

//        void Init() {
//            SocketClient.OnRegister();
//        }

//        public void OnInit() {
//            CallMethod("Start");
//        }

//        public void Unload() {
//            CallMethod("Unload");
//        }

//        /// <summary>
//        /// 执行Lua方法
//        /// </summary>
//        public object[] CallMethod(string func, params object[] args) {
//            return Util.CallMethod("Network", func, args);
//        }

//        ///------------------------------------------------------------------------------------
//        public static void AddEvent(int _event, ByteBuffer data) {
//            lock (m_lockObject) {
//                mEvents.Enqueue(new KeyValuePair<int, ByteBuffer>(_event, data));
//            }
//        }

//        /// <summary>
//        /// 交给Command，这里不想关心发给谁。
//        /// </summary>
//        void Update() {
//            if (mEvents.Count > 0) {
//                while (mEvents.Count > 0) {
//                    KeyValuePair<int, ByteBuffer> _event = mEvents.Dequeue();
//                    facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);
//                }
//            }
//        }

//        /// <summary>
//        /// 发送链接请求
//        /// </summary>
//        public void SendConnect() {
//            SocketClient.SendConnect();
//        }

//        /// <summary>
//        /// 发送SOCKET消息
//        /// </summary>
//        public void SendMessage(ByteBuffer buffer) {
//            SocketClient.SendMessage(buffer);
//        }

//        /// <summary>
//        /// 析构函数
//        /// </summary>
//        new void OnDestroy() {
//            SocketClient.OnRemove();
//            Debug.Log("~NetworkManager was destroy");
//        }
//    }
//}

//----------------------------------------原版LuaFramework.NetworkManager------------end---------------------------------------------------------------------