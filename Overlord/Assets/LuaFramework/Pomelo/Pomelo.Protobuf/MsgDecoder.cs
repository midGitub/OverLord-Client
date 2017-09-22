

using SimpleJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pomelo.Protobuf
{
    public class MsgDecoder
    {
        private JsonObject protos { get; set; }

        private int offset { get; set; }

        private byte[] buffer { get; set; }

        private Util util { get; set; }

        public MsgDecoder(JsonObject protos)
        {
            if (protos == null)
                protos = new JsonObject();
            this.protos = protos;
            this.util = new Util();
        }

        public JsonObject decode(string route, byte[] buf)
        {
            this.buffer = buf;
            this.offset = 0;
            object obj = (object)null;
            if (this.protos.TryGetValue(route, out obj))
                return this.decodeMsg(new JsonObject(), (JsonObject)obj, this.buffer.Length);
            return (JsonObject)null;
        }

        private JsonObject decodeMsg(JsonObject msg, JsonObject proto, int length)
        {
            while (this.offset < length)
            {
                int num;
                if (this.getHead().TryGetValue("tag", out num))
                {
                    object obj1 = (object)null;
                    object obj2;
                    object obj3;
                    object obj4;
                    if (proto.TryGetValue("__tags", out obj1) && ((JsonObject)obj1).TryGetValue(num.ToString(), out obj2) && (proto.TryGetValue(obj2.ToString(), out obj3) && ((JsonObject)obj3).TryGetValue("option", out obj4)))
                    {
                        switch (obj4.ToString())
                        {
                            case "optional":
                            case "required":
                                object obj5;
                                if (((JsonObject)obj3).TryGetValue("type", out obj5))
                                {
                                    msg.Add(obj2.ToString(), this.decodeProp(obj5.ToString(), proto));
                                    continue;
                                }
                                continue;
                            case "repeated":
                                object obj6;
                                if (!msg.TryGetValue(obj2.ToString(), out obj6))
                                    msg.Add(obj2.ToString(), (object)new List<object>());
                                object obj7;
                                if (msg.TryGetValue(obj2.ToString(), out obj6) && ((JsonObject)obj3).TryGetValue("type", out obj7))
                                {
                                    this.decodeArray((List<object>)obj6, obj7.ToString(), proto);
                                    continue;
                                }
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            return msg;
        }

        private void decodeArray(List<object> list, string type, JsonObject proto)
        {
            if (this.util.isSimpleType(type))
            {
                int num = (int)Decoder.decodeUInt32(this.getBytes());
                for (int index = 0; index < num; ++index)
                    list.Add(this.decodeProp(type, (JsonObject)null));
            }
            else
                list.Add(this.decodeProp(type, proto));
        }

        private object decodeProp(string type, JsonObject proto)
        {
            switch (type)
            {
                case "uInt32":
                    return (object)Decoder.decodeUInt32(this.getBytes());
                case "int32":
                case "sInt32":
                    return (object)Decoder.decodeSInt32(this.getBytes());
                case "float":
                    return (object)this.decodeFloat();
                case "double":
                    return (object)this.decodeDouble();
                case "string":
                    return (object)this.decodeString();
                default:
                    return (object)this.decodeObject(type, proto);
            }
        }

        private JsonObject decodeObject(string type, JsonObject proto)
        {
            object obj1;
            object obj2;
            if (proto == null || !proto.TryGetValue("__messages", out obj1) || !((JsonObject)obj1).TryGetValue(type, out obj2) && !this.protos.TryGetValue("message " + type, out obj2))
                return new JsonObject();
            int num = (int)Decoder.decodeUInt32(this.getBytes());
            return this.decodeMsg(new JsonObject(), (JsonObject)obj2, this.offset + num);
        }

        private string decodeString()
        {
            int count = (int)Decoder.decodeUInt32(this.getBytes());
            string @string = Encoding.UTF8.GetString(this.buffer, this.offset, count);
            this.offset += count;
            return @string;
        }

        private double decodeDouble()
        {
            double num = BitConverter.Int64BitsToDouble((long)this.ReadRawLittleEndian64());
            this.offset += 8;
            return num;
        }

        private float decodeFloat()
        {
            float num = BitConverter.ToSingle(this.buffer, this.offset);
            this.offset += 4;
            return num;
        }

        private ulong ReadRawLittleEndian64()
        {
            return (ulong)((long)this.buffer[this.offset] | (long)this.buffer[this.offset + 1] << 8 | (long)this.buffer[this.offset + 2] << 16 | (long)this.buffer[this.offset + 3] << 24 | (long)this.buffer[this.offset + 4] << 32 | (long)this.buffer[this.offset + 5] << 40 | (long)this.buffer[this.offset + 6] << 48 | (long)this.buffer[this.offset + 7] << 56);
        }

        private Dictionary<string, int> getHead()
        {
            int num = (int)Decoder.decodeUInt32(this.getBytes());
            return new Dictionary<string, int>()
      {
        {
          "type",
          num & 7
        },
        {
          "tag",
          num >> 3
        }
      };
        }

        private byte[] getBytes()
        {
            List<byte> list = new List<byte>();
            int offset = this.offset;
            byte num;
            do
            {
                num = this.buffer[offset];
                list.Add(num);
                ++offset;
            }
            while ((int)num >= 128);
            this.offset = offset;
            int count = list.Count;
            byte[] numArray = new byte[count];
            for (int index = 0; index < count; ++index)
                numArray[index] = list[index];
            return numArray;
        }
    }
}
