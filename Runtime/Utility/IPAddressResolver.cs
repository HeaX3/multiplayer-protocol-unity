using System.Net;
using UnityEngine;

namespace MultiplayerProtocol
{
    public static class IPAddressResolver
    {
        public static IPAddress Resolve(string address)
        {
            if (address == "0.0.0.0") return IPAddress.Any;
            
            var hosts = Dns.GetHostAddresses(address);
            if (hosts.Length == 0)
            {
                Debug.LogError("Cannot resolve host name " + address);
                return default;
            }

            return hosts[0];

        }
    }
}
