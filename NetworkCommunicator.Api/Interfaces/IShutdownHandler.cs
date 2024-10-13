using NetworkCommunicator.Api.Entities;

namespace NetworkCommunicator.Api.Interfaces
{
    public interface IShutdownHandler
    {
        int Shutdown(NetworkDetail network);
    }
}
