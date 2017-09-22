
using SimpleJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pomelo.Protobuf
{
    public class MsgEncoder
    {
        private JsonObject protos { get; set; }

        private Encoder encoder { get; set; }

        private Util util { get; set; }

        public MsgEncoder(JsonObject protos)
        {
            if (protos == null)
                protos = new JsonObject();
            this.protos = protos;
            this.util = new Util();
        }

        public byte[] encode(string route, JsonObject msg)
        {
            byte[] numArray = (byte[])null;
            object obj;
            if (this.protos.TryGetValue(route, out obj))
            {
                if (!this.checkMsg(msg, (JsonObject)obj))
                    return (byte[])null;
                int length1 = Encoder.byteLength(msg.ToString()) * 2;
                int offset = 0;
                byte[] buffer = new byte[length1];
                int length2 = this.encodeMsg(buffer, offset, (JsonObject)obj, msg);
                numArray = new byte[length2];
                for (int index = 0; index < length2; ++index)
                    numArray[index] = buffer[index];
            }
            return numArray;
        }

        private bool checkMsg(JsonObject msg, JsonObject proto)
        {
            foreach (string key in (IEnumerable<string>)proto.Keys)
            {
                JsonObject jsonObject1 = (JsonObject)proto[key];
                object obj1;
                object obj2;
                if (jsonObject1.TryGetValue("option", out obj1))
                {
                    switch (obj1.ToString())
                    {
                        case "required":
                            if (!msg.ContainsKey(key))
                                return false;
                            continue;
                        case "optional":
                            JsonObject jsonObject2 = (JsonObject)proto["__messages"];
                            obj2 = jsonObject1["type"];
                            object obj3;
                            if (msg.ContainsKey(key) && (jsonObject2.TryGetValue(obj2.ToString(), out obj3) || this.protos.TryGetValue("message " + obj2.ToString(), out obj3)))
                            {
                                this.checkMsg((JsonObject)msg[key], (JsonObject)obj3);
                                continue;
                            }
                            continue;
                        case "repeated":
                            object obj4;
                            object obj5;
                            if (jsonObject1.TryGetValue("type", out obj2) && msg.TryGetValue(key, out obj4) && (((JsonObject)proto["__messages"]).TryGetValue(obj2.ToString(), out obj5) || this.protos.TryGetValue("message " + obj2.ToString(), out obj5)))
                            {
                                using (List<object>.Enumerator enumerator = ((List<object>)obj4).GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        if (!this.checkMsg((JsonObject)enumerator.Current, (JsonObject)obj5))
                                            return false;
                                    }
                                    continue;
                                }
                            }
                            else
                                continue;
                        default:
                            continue;
                    }
                }
            }
            return true;
        }

        private int encodeMsg(byte[] buffer, int offset, JsonObject proto, JsonObject msg)
        {
            foreach (string key in (IEnumerable<string>)msg.Keys)
            {
                object obj1;
                object obj2;
                if (proto.TryGetValue(key, out obj1) && ((JsonObject)obj1).TryGetValue("option", out obj2))
                {
                    switch (obj2.ToString())
                    {
                        case "required":
                        case "optional":
                            object obj3;
                            object obj4;
                            if (((JsonObject)obj1).TryGetValue("type", out obj3) && ((JsonObject)obj1).TryGetValue("tag", out obj4))
                            {
                                offset = this.writeBytes(buffer, offset, this.encodeTag(obj3.ToString(), Convert.ToInt32(obj4)));
                                offset = this.encodeProp(msg[key], obj3.ToString(), offset, buffer, proto);
                                continue;
                            }
                            continue;
                        case "repeated":
                            object obj5;
                            if (msg.TryGetValue(key, out obj5) && ((List<object>)obj5).Count > 0)
                            {
                                offset = this.encodeArray((List<object>)obj5, (JsonObject)obj1, offset, buffer, proto);
                                continue;
                            }
                            continue;
                        default:
                            continue;
                    }
                }
            }
            return offset;
        }

        private int encodeArray(List<object> msg, JsonObject value, int offset, byte[] buffer, JsonObject proto)
        {
            object obj1;
            object obj2;
            if (value.TryGetValue("type", out obj1) && value.TryGetValue("tag", out obj2))
            {
                if (this.util.isSimpleType(obj1.ToString()))
                {
                    offset = this.writeBytes(buffer, offset, this.encodeTag(obj1.ToString(), Convert.ToInt32(obj2)));
                    offset = this.writeBytes(buffer, offset, Encoder.encodeUInt32((uint)msg.Count));
                    foreach (object obj3 in msg)
                        offset = this.encodeProp(obj3, obj1.ToString(), offset, buffer, (JsonObject)null);
                }
                else
                {
                    foreach (object obj3 in msg)
                    {
                        offset = this.writeBytes(buffer, offset, this.encodeTag(obj1.ToString(), Convert.ToInt32(obj2)));
                        offset = this.encodeProp(obj3, obj1.ToString(), offset, buffer, proto);
                    }
                }
            }
            return offset;
        }

        private int encodeProp(object value, string type, int offset, byte[] buffer, JsonObject proto)
        {
            switch (type)
            {
                case "uInt32":
                    this.writeUInt32(buffer, ref offset, value);
                    break;
                case "int32":
                case "sInt32":
                    this.writeInt32(buffer, ref offset, value);
                    break;
                case "float":
                    this.writeFloat(buffer, ref offset, value);
                    break;
                case "double":
                    this.writeDouble(buffer, ref offset, value);
                    break;
                case "string":
                    this.writeString(buffer, ref offset, value);
                    break;
                default:
                    object obj1;
                    object obj2;
                    if (proto.TryGetValue("__messages", out obj1) && (((JsonObject)obj1).TryGetValue(type, out obj2) || this.protos.TryGetValue("message " + type, out obj2)))
                    {
                        byte[] buffer1 = new byte[Encoder.byteLength(value.ToString()) * 3];
                        int offset1 = 0;
                        int num = this.encodeMsg(buffer1, offset1, (JsonObject)obj2, (JsonObject)value);
                        offset = this.writeBytes(buffer, offset, Encoder.encodeUInt32((uint)num));
                        for (int index = 0; index < num; ++index)
                        {
                            buffer[offset] = buffer1[index];
                            ++offset;
                        }
                        break;
                    }
                    break;
            }
            return offset;
        }

        private void writeString(byte[] buffer, ref int offset, object value)
        {
            int byteCount = Encoding.UTF8.GetByteCount(value.ToString());
            offset = this.writeBytes(buffer, offset, Encoder.encodeUInt32((uint)byteCount));
            byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
            this.writeBytes(buffer, offset, bytes);
            offset += byteCount;
        }

        private void writeDouble(byte[] buffer, ref int offset, object value)
        {
            this.WriteRawLittleEndian64(buffer, offset, (ulong)BitConverter.DoubleToInt64Bits(double.Parse(value.ToString())));
            offset += 8;
        }

        private void writeFloat(byte[] buffer, ref int offset, object value)
        {
            this.writeBytes(buffer, offset, Encoder.encodeFloat(float.Parse(value.ToString())));
            offset += 4;
        }

        private void writeUInt32(byte[] buffer, ref int offset, object value)
        {
            offset = this.writeBytes(buffer, offset, Encoder.encodeUInt32(value.ToString()));
        }

        private void writeInt32(byte[] buffer, ref int offset, object value)
        {
            offset = this.writeBytes(buffer, offset, Encoder.encodeSInt32(value.ToString()));
        }

        private int writeBytes(byte[] buffer, int offset, byte[] bytes)
        {
            for (int index = 0; index < bytes.Length; ++index)
            {
                buffer[offset] = bytes[index];
                ++offset;
            }
            return offset;
        }

        private byte[] encodeTag(string type, int tag)
        {
            int num = this.util.containType(type);
            return Encoder.encodeUInt32((uint)(tag << 3 | num));
        }

        private void WriteRawLittleEndian64(byte[] buffer, int offset, ulong value)
        {
            buffer[offset++] = (byte)value;
            buffer[offset++] = (byte)(value >> 8);
            buffer[offset++] = (byte)(value >> 16);
            buffer[offset++] = (byte)(value >> 24);
            buffer[offset++] = (byte)(value >> 32);
            buffer[offset++] = (byte)(value >> 40);
            buffer[offset++] = (byte)(value >> 48);
            buffer[offset++] = (byte)(value >> 56);
        }
    }
}
