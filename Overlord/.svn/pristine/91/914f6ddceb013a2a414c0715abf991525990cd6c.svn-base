
using System.Collections.Generic;

namespace Pomelo.Protobuf
{
    public class Util
    {
        private string[] types;
        private Dictionary<string, int> typeMap;

        public Util()
        {
            this.initTypeMap();
            this.types = new string[7]
      {
        "uInt32",
        "sInt32",
        "int32",
        "uInt64",
        "sInt64",
        "float",
        "double"
      };
        }

        public bool isSimpleType(string type)
        {
            int length = this.types.Length;
            bool flag = false;
            for (int index = 0; index < length; ++index)
            {
                if (type == this.types[index])
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public int containType(string type)
        {
            int num1 = 2;
            int num2;
            if (this.typeMap.TryGetValue(type, out num2))
                num1 = num2;
            return num1;
        }

        private void initTypeMap()
        {
            this.typeMap = new Dictionary<string, int>()
      {
        {
          "uInt32",
          0
        },
        {
          "sInt32",
          0
        },
        {
          "int32",
          0
        },
        {
          "double",
          1
        },
        {
          "string",
          2
        },
        {
          "float",
          5
        },
        {
          "message",
          2
        }
      };
        }

        public static void Reverse(byte[] bytes)
        {
            int index1 = 0;
            for (int index2 = bytes.Length - 1; index1 < index2; --index2)
            {
                byte num = bytes[index1];
                bytes[index1] = bytes[index2];
                bytes[index2] = num;
                ++index1;
            }
        }
    }
}
