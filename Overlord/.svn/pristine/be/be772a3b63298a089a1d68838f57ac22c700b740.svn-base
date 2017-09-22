

using Pomelo.Protobuf;
using SimpleJson;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pomelo.Protobuf.Test
{
    public class ProtobufTest
    {
        public static JsonObject read(string name)
        {
            return (JsonObject)SimpleJson.SimpleJson.DeserializeObject(new StreamReader(name).ReadToEnd());
        }

        public static bool equal(JsonObject a, JsonObject b)
        {
            ICollection<string> keys1 = a.Keys;
            ICollection<string> keys2 = b.Keys;
            foreach (string index in (IEnumerable<string>)keys1)
            {
                Console.WriteLine((object)a[index].GetType());
                if (a[index].GetType().ToString() == "SimpleJson.JsonObject")
                {
                    if (!ProtobufTest.equal((JsonObject)a[index], (JsonObject)b[index]))
                        return false;
                }
                else if (!(a[index].GetType().ToString() == "SimpleJson.JsonArray") && !a[index].ToString().Equals(b[index].ToString()))
                    return false;
            }
            return true;
        }

        public static void Run()
        {
            JsonObject jsonObject1 = ProtobufTest.read("../../json/rootProtos.json");
            JsonObject jsonObject2 = ProtobufTest.read("../../json/rootMsg.json");
            Protobuf protobuf = new Protobuf(jsonObject1, jsonObject1);
            foreach (string route in (IEnumerable<string>)jsonObject2.Keys)
            {
                JsonObject jsonObject3 = (JsonObject)jsonObject2[route];
                byte[] buffer = protobuf.encode(route, jsonObject3);
                JsonObject b = protobuf.decode(route, buffer);
                if (!ProtobufTest.equal(jsonObject3, b))
                {
                    Console.WriteLine("protobuf test failed!");
                    return;
                }
            }
            Console.WriteLine("Protobuf test success!");
        }

        private static void print(byte[] bytes, int offset, int length)
        {
            for (int index = offset; index < length; ++index)
                Console.Write(Convert.ToString(bytes[index], 16) + " ");
            Console.WriteLine();
        }
    }
}
