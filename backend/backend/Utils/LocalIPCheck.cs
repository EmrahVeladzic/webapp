using System.Net;
using System.Net.NetworkInformation;

namespace backend.Utils
{
    public static class LocalIPCheck
    {

        public static bool IPIsLocal(IPAddress ip)
        {
            if (IPAddress.IsLoopback(ip))
            {
                return true;
            }


            foreach (NetworkInterface ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ua in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.Equals(ip)) return true;
                }
            }

            return false;
        }



    }
}
