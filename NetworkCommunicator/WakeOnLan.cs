using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace NetworkCommunicator
{
    public class WakeOnLan
    {
        public static async Task Wake(string macAddress)
        {
            byte[] magicPacket = BuildMagicPacket(macAddress);

            IEnumerable<NetworkInterface> interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback);

            foreach (NetworkInterface nic in interfaces)
            {
                IPInterfaceProperties interfaceProperties = nic.GetIPProperties();
                UnicastIPAddressInformation? unicastIPAddressInformation = interfaceProperties.UnicastAddresses
                    .Where(u => u.Address.AddressFamily == AddressFamily.InterNetwork)
                    .FirstOrDefault();
                if (unicastIPAddressInformation != null)
                {
                    await SendMagicPacket(magicPacket, unicastIPAddressInformation.Address, new IPAddress(new byte[] {224, 0, 0, 1}));
                    return;
                }
            }
        }

        private static byte[] BuildMagicPacket(string macAddress) 
        {
            macAddress = Regex.Replace(macAddress, "[: -]", "");
            byte[] macBytes = Convert.FromHexString(macAddress);

            IEnumerable<byte> header = Enumerable.Repeat((byte)0xff, 6);
            IEnumerable<byte> data = Enumerable.Repeat(macBytes, 16).SelectMany(x => x);

            return header.Concat(data).ToArray();
        }

        private static async Task SendMagicPacket(byte[] magicPacket, IPAddress localIpAddress, IPAddress multicastIpAddress)
        {
            using (UdpClient udpClient = new UdpClient(new IPEndPoint(localIpAddress, 0)))
            {
                await udpClient.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(multicastIpAddress, 7));
            }
        }
    }
}
