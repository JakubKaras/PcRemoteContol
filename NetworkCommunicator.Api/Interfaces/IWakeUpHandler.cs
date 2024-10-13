using NetworkCommunicator.Api.Models;

namespace NetworkCommunicator.Api.Interfaces
{
    public interface IWakeUpHandler
    {
        Task WakeUp(NetworkDetail device);
    }
}
