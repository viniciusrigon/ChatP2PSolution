using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace br.chatp2p.Networking
{
    public static class EnderecoIP
    {
        public static IPEndPoint ParseEndPoint(string endereco)
        {
            int pos = endereco.LastIndexOfAny(":".ToCharArray());
            string strIP = endereco.Substring(0, pos);
            string strPorta = endereco.Substring(pos+1);
            IPAddress ipOut = IPAddress.Parse(strIP);
            int porta = Int32.Parse(strPorta);
            IPEndPoint ret = new IPEndPoint(ipOut, porta);
            return ret;
        }

        // fonte: msdn
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }
    }
}
