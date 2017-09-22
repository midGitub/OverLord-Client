

using Pomelo.Protobuf;
using System;

namespace Pomelo.Protobuf.Test
{
    public class CodecTest
    {
        public static bool EncodeSInt32Test(int count)
        {
            Random random = new Random();
            int num1 = -1;
            for (int index = 0; index < count; ++index)
            {
                num1 *= -1;
                int n = random.Next(0, int.MaxValue) * num1;
                int num2 = Decoder.decodeSInt32(Encoder.encodeSInt32(n));
                if (n != num2)
                    return false;
            }
            return true;
        }

        public static bool EncodeUInt32Test(int count)
        {
            Random random = new Random();
            for (int index = 0; index < count; ++index)
            {
                uint n = (uint)random.Next(0, int.MaxValue);
                uint num = Decoder.decodeUInt32(Encoder.encodeUInt32(n));
                if ((int)n != (int)num)
                    return false;
            }
            return true;
        }

        public static bool Run()
        {
            bool flag1 = true;
            DateTime now1 = DateTime.Now;
            bool flag2 = CodecTest.EncodeSInt32Test(10000);
            Console.WriteLine("Encode sint32 test finished , result is : {1}, cost time : {0}", (object)(DateTime.Now - now1), (object)(flag2 ? true : false));
            if (!flag2)
                flag1 = false;
            DateTime now2 = DateTime.Now;
            bool flag3 = CodecTest.EncodeUInt32Test(10000);
            Console.WriteLine("Encode uint32 test finished , result is : {1}, cost time : {0}", (object)(DateTime.Now - now2), (object)(flag3 ? true : false));
            if (!flag3)
                flag1 = false;
            return flag1;
        }
    }
}
