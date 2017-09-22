
using Pomelo.Protobuf;
using SimpleJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pomelo.DotNetClient
{
    public class MessageProtocol
    {
        private Dictionary<string, ushort> dict = new Dictionary<string, ushort>();
        private Dictionary<ushort, string> abbrs = new Dictionary<ushort, string>();
        private JsonObject encodeProtos = new JsonObject();
        private JsonObject decodeProtos = new JsonObject();
        public const int MSG_Route_Limit = 255;
        public const int MSG_Route_Mask = 1;
        public const int MSG_Type_Mask = 7;
        private Dictionary<uint, string> reqMap;
        private Protobuf.Protobuf protobuf;

        public MessageProtocol(JsonObject dict, JsonObject serverProtos, JsonObject clientProtos)
        {
            foreach (string index1 in (IEnumerable<string>)dict.Keys)
            {
                ushort index2 = Convert.ToUInt16(dict[index1]);
                this.dict[index1] = index2;
                this.abbrs[index2] = index1;
            }
            this.protobuf = new Protobuf.Protobuf(clientProtos, serverProtos);
            this.encodeProtos = clientProtos;
            this.decodeProtos = serverProtos;
            this.reqMap = new Dictionary<uint, string>();
        }

        public byte[] encode(string route, JsonObject msg)
        {
            return this.encode(route, 0U, msg);
        }

        public byte[] encode(string route, uint id, JsonObject msg)
        {
            int num1 = this.byteLength(route);
            if (num1 > (int)byte.MaxValue)
                throw new Exception("Route is too long!");
            byte[] numArray1 = new byte[num1 + 6];
            int offset1 = 1;
            byte num2 = (byte)0;
            byte num3;
            if (id > 0U)
            {
                byte[] source = Pomelo.Protobuf.Encoder.encodeUInt32(id);
                this.writeBytes(source, offset1, numArray1);
                num3 = num2;
                offset1 += source.Length;
            }
            else
                num3 = (byte)((uint)num2 | 2U);
            int num4;
            if (this.dict.ContainsKey(route))
            {
                ushort num5 = this.dict[route];
                this.writeShort(offset1, num5, numArray1);
                num3 |= (byte)1;
                num4 = offset1 + 2;
            }
            else
            {
                byte[] numArray2 = numArray1;
                int index = offset1;
                int num5 = 1;
                int offset2 = index + num5;
                int num6 = (int)(byte)num1;
                numArray2[index] = (byte)num6;
                this.writeBytes(Encoding.UTF8.GetBytes(route), offset2, numArray1);
                num4 = offset2 + num1;
            }
            numArray1[0] = num3;
            byte[] numArray3 = !this.encodeProtos.ContainsKey(route) ? Encoding.UTF8.GetBytes(msg.ToString()) : this.protobuf.encode(route, msg);
            byte[] numArray4 = new byte[num4 + numArray3.Length];
            for (int index = 0; index < num4; ++index)
                numArray4[index] = numArray1[index];
            for (int index = 0; index < numArray3.Length; ++index)
                numArray4[num4 + index] = numArray3[index];
            if (id > 0U)
                this.reqMap.Add(id, route);
            return numArray4;
        }

        public Message decode(byte[] buffer)
        {
            byte num1 = buffer[0];
            int offset = 1;
            MessageType type = (MessageType)((int)num1 >> 1 & 7);
            uint index1 = 0U;
            string str;
            int num2;
            if (type == MessageType.MSG_RESPONSE)
            {
                int length;
                index1 = Pomelo.Protobuf.Decoder.decodeUInt32(offset, buffer, out length);
                if (index1 <= 0U || !this.reqMap.ContainsKey(index1))
                    return (Message)null;
                str = this.reqMap[index1];
                this.reqMap.Remove(index1);
                num2 = offset + length;
            }
            else
            {
                if (type != MessageType.MSG_PUSH)
                    return (Message)null;
                if (((int)num1 & 1) == 1)
                {
                    str = this.abbrs[this.readShort(offset, buffer)];
                    num2 = offset + 2;
                }
                else
                {
                    byte num3 = buffer[offset];
                    int index2 = offset + 1;
                    str = Encoding.UTF8.GetString(buffer, index2, (int)num3);
                    num2 = index2 + (int)num3;
                }
            }
            byte[] numArray = new byte[buffer.Length - num2];
            for (int index2 = 0; index2 < numArray.Length; ++index2)
                numArray[index2] = buffer[index2 + num2];
            JsonObject data = !this.decodeProtos.ContainsKey(str) ? (JsonObject)SimpleJson.SimpleJson.DeserializeObject(Encoding.UTF8.GetString(numArray)) : this.protobuf.decode(str, numArray);
            return new Message(type, index1, str, data);
        }

        private void writeInt(int offset, uint value, byte[] bytes)
        {
            bytes[offset] = (byte)(value >> 24 & (uint)byte.MaxValue);
            bytes[offset + 1] = (byte)(value >> 16 & (uint)byte.MaxValue);
            bytes[offset + 2] = (byte)(value >> 8 & (uint)byte.MaxValue);
            bytes[offset + 3] = (byte)(value & (uint)byte.MaxValue);
        }

        private void writeShort(int offset, ushort value, byte[] bytes)
        {
            bytes[offset] = (byte)((int)value >> 8 & (int)byte.MaxValue);
            bytes[offset + 1] = (byte)((uint)value & (uint)byte.MaxValue);
        }

        private ushort readShort(int offset, byte[] bytes)
        {
            return (ushort)((uint)(ushort)(0U + (uint)(ushort)((uint)bytes[offset] << 8)) + (uint)bytes[offset + 1]);
        }

        private int byteLength(string msg)
        {
            return Encoding.UTF8.GetBytes(msg).Length;
        }

        private void writeBytes(byte[] source, int offset, byte[] target)
        {
            for (int index = 0; index < source.Length; ++index)
                target[offset + index] = source[index];
        }
    }
}
