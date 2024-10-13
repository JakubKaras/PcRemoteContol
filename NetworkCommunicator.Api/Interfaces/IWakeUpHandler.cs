using NetworkCommunicator.Api.Entities;

namespace NetworkCommunicator.Api.Interfaces
{
    public interface IWakeUpHandler
    {
        Task WakeUp(NetworkDetail device);
    }
}
