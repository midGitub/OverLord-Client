

using SimpleJson;
using System;
using System.Net.Sockets;
using System.Text;

namespace Pomelo.DotNetClient
{
    public class Protocol
    {
        private MessageProtocol messageProtocol;
        private ProtocolState state;
        private Transporter transporter;
        private HandShakeService handshake;
        private HeartBeatService heartBeatService;
        private PomeloClient pc;

        public Protocol(PomeloClient pc, Socket socket)
        {
            this.pc = pc;
            this.transporter = new Transporter(socket, new Action<byte[]>(this.processMessage));
            this.transporter.onDisconnect = new Action(this.onDisconnect);
            this.handshake = new HandShakeService(this);
            this.state = ProtocolState.start;
        }

        public PomeloClient getPomeloClient()
        {
            return this.pc;
        }

        internal void start(JsonObject user, Action<JsonObject> callback)
        {
            this.transporter.start();
            this.handshake.request(user, callback);
            this.state = ProtocolState.handshaking;
        }

        internal void send(string route, JsonObject msg)
        {
            this.send(route, 0U, msg);
        }

        internal void send(string route, uint id, JsonObject msg)
        {
            if (this.state != ProtocolState.working)
                return;
            this.send(PackageType.PKG_DATA, this.messageProtocol.encode(route, id, msg));
        }

        internal void send(PackageType type)
        {
            if (this.state == ProtocolState.closed)
                return;
            this.transporter.send(PackageProtocol.encode(type));
        }

        internal void send(PackageType type, JsonObject msg)
        {
            if (type == PackageType.PKG_DATA)
                return;
            byte[] bytes = Encoding.UTF8.GetBytes(msg.ToString());
            this.send(type, bytes);
        }

        internal void send(PackageType type, byte[] body)
        {
            if (this.state == ProtocolState.closed)
                return;
            this.transporter.send(PackageProtocol.encode(type, body));
        }

        internal void processMessage(byte[] bytes)
        {
            Package package = PackageProtocol.decode(bytes);
            if (package.type == PackageType.PKG_HANDSHAKE && this.state == ProtocolState.handshaking)
            {
                this.processHandshakeData((JsonObject)SimpleJson.SimpleJson.DeserializeObject(Encoding.UTF8.GetString(package.body)));
                this.state = ProtocolState.working;
            }
            else if (package.type == PackageType.PKG_HEARTBEAT && this.state == ProtocolState.working)
                this.heartBeatService.resetTimeout();
            else if (package.type == PackageType.PKG_DATA && this.state == ProtocolState.working)
            {
                this.heartBeatService.resetTimeout();
                this.pc.processMessage(this.messageProtocol.decode(package.body));
            }
            else
            {
                if (package.type != PackageType.PKG_KICK)
                    return;
                this.getPomeloClient().disconnect();
                this.close();
            }
        }

        private void processHandshakeData(JsonObject msg)
        {
            if (!msg.ContainsKey("code") || !msg.ContainsKey("sys") || Convert.ToInt32(msg["code"]) != 200)
                throw new Exception("Handshake error! Please check your handshake config.");
            JsonObject jsonObject1 = (JsonObject)msg["sys"];
            JsonObject dict = new JsonObject();
            if (jsonObject1.ContainsKey("dict"))
                dict = (JsonObject)jsonObject1["dict"];
            JsonObject jsonObject2 = new JsonObject();
            JsonObject serverProtos = new JsonObject();
            JsonObject clientProtos = new JsonObject();
            if (jsonObject1.ContainsKey("protos"))
            {
                JsonObject jsonObject3 = (JsonObject)jsonObject1["protos"];
                serverProtos = (JsonObject)jsonObject3["server"];
                clientProtos = (JsonObject)jsonObject3["client"];
            }
            this.messageProtocol = new MessageProtocol(dict, serverProtos, clientProtos);
            int interval = 0;
            if (jsonObject1.ContainsKey("heartbeat"))
                interval = Convert.ToInt32(jsonObject1["heartbeat"]);
            this.heartBeatService = new HeartBeatService(interval, this);
            if (interval > 0)
                this.heartBeatService.start();
            this.handshake.ack();
            this.state = ProtocolState.working;
            JsonObject data = new JsonObject();
            if (msg.ContainsKey("user"))
                data = (JsonObject)msg["user"];
            this.handshake.invokeCallback(data);
        }

        private void onDisconnect()
        {
            this.pc.disconnect();
        }

        internal void close()
        {
            this.transporter.close();
            if (this.heartBeatService != null)
                this.heartBeatService.stop();
            this.state = ProtocolState.closed;
        }
    }
}
