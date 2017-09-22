

using System;
using System.Collections.Generic;
using System.Text;

namespace Pomelo.Protobuf
{
    public class Encoder
    {
        public static byte[] encodeUInt32(string n)
        {
            return Encoder.encodeUInt32(Convert.ToUInt32(n));
        }

        public static byte[] encodeUInt32(uint n)
        {
            List<byte> list = new List<byte>();
            do
            {
                uint num1 = n % 128U;
                uint num2 = n >> 7;
                if ((int)num2 != 0)
                    num1 += 128U;
                list.Add(Convert.ToByte(num1));
                n = num2;
            }
            while ((int)n != 0);
            return list.ToArray();
        }

        public static byte[] encodeSInt32(string n)
        {
            return Encoder.encodeSInt32(Convert.ToInt32(n));
        }

        public static byte[] encodeSInt32(int n)
        {
            return Encoder.encodeUInt32(n < 0 ? (uint)(Math.Abs(n) * 2 - 1) : (uint)(n * 2));
        }

        public static byte[] encodeFloat(float n)
        {
            byte[] bytes = BitConverter.GetBytes(n);
            if (!BitConverter.IsLittleEndian)
                Util.Reverse(bytes);
            return bytes;
        }

        public static int byteLength(string msg)
        {
            return Encoding.UTF8.GetBytes(msg).Length;
        }
    }
}
