using NetworkCommunicator.Api.Models;

namespace NetworkCommunicator.Api.Interfaces
{
    public interface IShutdownHandler
    {
        int Shutdown(NetworkDetail network);
    }
}
