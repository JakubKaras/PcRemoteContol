using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace NetworkCommunicator
{
    public static class NetworkDetailExtensions
    {
        const int ShutdownPort = 9110;
        private static readonly byte[] _shutdowMessage = { 115, 104, 117, 116, 100, 111, 119, 110, 10 };

        public static async Task Wake(this NetworkDetail networkDetail)
        {
            byte[] magicPacket = BuildMagicPacket(networkDetail.MacAddress);

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
                    await SendMagicPacket(magicPacket, unicastIPAddressInformation.Address, new IPAddress(new byte[] { 224, 0, 0, 1 }));
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

        public static int Shutdown(this NetworkDetail network)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPAddress.Parse(network.IpAddress), ShutdownPort);
                int result = socket.Send(_shutdowMessage);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public static async Task<bool> Ping(this NetworkDetail networkDetail)
        {
            Ping pinger = new();
            var isOnline = false;

            try
            {
                networkDetail.Status = DeviceStatus.Loading;
                var reply = await pinger.SendPingAsync(networkDetail.IpAddress);
                isOnline = reply.Status == IPStatus.Success;
            }
            finally
            {
                networkDetail.Status = isOnline ? DeviceStatus.Online : DeviceStatus.Offline;
                pinger?.Dispose();
            }


            return isOnline;
        }
    }
}
