using NetworkCommunicator.Api.Interfaces;
using NetworkCommunicator.PingHandlers;
using NetworkCommunicator.ShutdownHandlers;
using NetworkCommunicator.WakeUpHandlers;

namespace NetworkCommunicator
{
    public static class NetworkCommunicatorInstaller
    {
        public static IServiceCollection InstallNetworkCommunicator(this IServiceCollection services)
        {
            return services
                    .AddSingleton<IWakeUpHandler, DefaultWakeUpHandler>()
                    .AddSingleton<IShutdownHandler, DefaultShutdownHandler>()
                    .AddSingleton<IPingHandler, DefaultPingHandler>();
        }
    }
}
