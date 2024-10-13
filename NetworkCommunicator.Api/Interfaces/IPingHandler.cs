using NetworkCommunicator.Api.Entities;

namespace NetworkCommunicator.Api.Interfaces
{
    public interface IPingHandler
    {
        Task<bool> Ping(NetworkDetail networkDetail);
    }
}
