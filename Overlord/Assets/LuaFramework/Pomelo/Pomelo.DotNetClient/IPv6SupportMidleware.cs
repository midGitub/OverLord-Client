
using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Pomelo.DotNetClient
{
    public class IPv6SupportMidleware
    {
        private static string GetIPv6(string mHost, string mPort)
        {
            return mHost + "&&ipv4";
        }

        public static void getIPType(string serverIp, string serverPorts, out string newServerIp, out AddressFamily mIPType)
        {
            mIPType = AddressFamily.InterNetwork;
            newServerIp = serverIp;
            try
            {
                string ipv6 = IPv6SupportMidleware.GetIPv6(serverIp, serverPorts);
                if (string.IsNullOrEmpty(ipv6))
                    return;
                string[] strArray = Regex.Split(ipv6, "&&");
                if (strArray == null || strArray.Length < 2 || !(strArray[1] == "ipv6"))
                    return;
                newServerIp = strArray[0];
                mIPType = AddressFamily.InterNetworkV6;
            }
            catch (Exception ex)
            {
                Debug.Log((object)("GetIPv6 error:" + (object)ex));
            }
        }
    }
}
