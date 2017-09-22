

using System;

namespace Pomelo.DotNetClient
{
    public class PackageProtocol
    {
        public const int HEADER_LENGTH = 4;

        public static byte[] encode(PackageType type)
        {
            byte[] numArray = new byte[4];
            numArray[0] = Convert.ToByte((object)type);
            return numArray;
        }

        public static byte[] encode(PackageType type, byte[] body)
        {
            int length = 4;
            if (body != null)
                length += body.Length;
            byte[] numArray1 = new byte[length];
            int num1 = 0;
            byte[] numArray2 = numArray1;
            int index1 = num1;
            int num2 = 1;
            int num3 = index1 + num2;
            int num4 = (int)Convert.ToByte((object)type);
            numArray2[index1] = (byte)num4;
            byte[] numArray3 = numArray1;
            int index2 = num3;
            int num5 = 1;
            int num6 = index2 + num5;
            int num7 = (int)Convert.ToByte(body.Length >> 16 & (int)byte.MaxValue);
            numArray3[index2] = (byte)num7;
            byte[] numArray4 = numArray1;
            int index3 = num6;
            int num8 = 1;
            int num9 = index3 + num8;
            int num10 = (int)Convert.ToByte(body.Length >> 8 & (int)byte.MaxValue);
            numArray4[index3] = (byte)num10;
            byte[] numArray5 = numArray1;
            int index4 = num9;
            int num11 = 1;
            int index5 = index4 + num11;
            int num12 = (int)Convert.ToByte(body.Length & (int)byte.MaxValue);
            numArray5[index4] = (byte)num12;
            for (; index5 < length; ++index5)
                numArray1[index5] = body[index5 - 4];
            return numArray1;
        }

        public static Package decode(byte[] buf)
        {
            PackageType type = (PackageType)buf[0];
            byte[] body = new byte[buf.Length - 4];
            for (int index = 0; index < body.Length; ++index)
                body[index] = buf[index + 4];
            return new Package(type, body);
        }
    }
}
