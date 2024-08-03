using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace NetworkCommunicator
{
    public class ShutdownOnLan
    {
        const int PORT = 9110;
        private static readonly byte[] _message = { 115, 104, 117, 116, 100, 111, 119, 110, 10 };
        public static int Shutdown(string ipAddress)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPAddress.Parse(ipAddress), PORT);
                int result = socket.Send(_message);
                return result;
            }
            catch
            {
                return -1;
            }
        }
    }
}
