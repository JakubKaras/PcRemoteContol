using NetworkCommunicator.Api.Models;

namespace NetworkCommunicator.Api.Interfaces
{
    public interface IPingHandler
    {
        Task<bool> Ping(NetworkDetail networkDetail);
    }
}
