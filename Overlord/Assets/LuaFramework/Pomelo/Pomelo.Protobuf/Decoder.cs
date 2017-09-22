
using System;

namespace Pomelo.Protobuf
{
    public class Decoder
    {
        public static uint decodeUInt32(int offset, byte[] bytes, out int length)
        {
            uint num1 = 0U;
            length = 0;
            for (int index = offset; index < bytes.Length; ++index)
            {
                ++length;
                uint num2 = Convert.ToUInt32(bytes[index]);
                num1 += Convert.ToUInt32((double)(num2 & (uint)sbyte.MaxValue) * Math.Pow(2.0, (double)(7 * (index - offset))));
                if (num2 < 128U)
                    break;
            }
            return num1;
        }

        public static uint decodeUInt32(byte[] bytes)
        {
            int length;
            return Decoder.decodeUInt32(0, bytes, out length);
        }

        public static int decodeSInt32(byte[] bytes)
        {
            uint num1 = Decoder.decodeUInt32(bytes);
            int num2 = (int)(num1 % 2U) == 1 ? -1 : 1;
            return Convert.ToInt32((long)((num1 % 2U + num1) / 2U) * (long)num2);
        }
    }
}
