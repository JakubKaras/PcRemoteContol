using NetworkCommunicator.Api.Entities;
using NetworkCommunicator.Api.Interfaces;
using System.Net.Sockets;
using System.Net;

namespace NetworkCommunicator.ShutdownHandlers
{
    internal class DefaultShutdownHandler : IShutdownHandler
    {
        const int ShutdownPort = 9110;
        private static readonly byte[] _shutdowMessage = { 115, 104, 117, 116, 100, 111, 119, 110, 10 };

        public int Shutdown(NetworkDetail network)
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
    }
}
