
using SimpleJson;
using System;
using System.Text;

namespace Pomelo.DotNetClient
{
    public class HandShakeService
    {
        public const string Version = "0.3.0";
        public const string Type = "unity-socket";
        private Protocol protocol;
        private Action<JsonObject> callback;

        public HandShakeService(Protocol protocol)
        {
            this.protocol = protocol;
        }

        public void request(JsonObject user, Action<JsonObject> callback)
        {
            this.protocol.send(PackageType.PKG_HANDSHAKE, Encoding.UTF8.GetBytes(this.buildMsg(user).ToString()));
            this.callback = callback;
        }

        internal void invokeCallback(JsonObject data)
        {
            if (this.callback == null)
                return;
            this.callback(data);
        }

        public void ack()
        {
            this.protocol.send(PackageType.PKG_HANDSHAKE_ACK, new byte[0]);
        }

        private JsonObject buildMsg(JsonObject user)
        {
            if (user == null)
                user = new JsonObject();
            JsonObject jsonObject1 = new JsonObject();
            JsonObject jsonObject2 = new JsonObject();
            jsonObject2["version"] = (object)"0.3.0";
            jsonObject2["type"] = (object)"unity-socket";
            jsonObject1["sys"] = (object)jsonObject2;
            jsonObject1["user"] = (object)user;
            return jsonObject1;
        }
    }
}
