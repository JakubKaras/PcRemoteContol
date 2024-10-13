using Microsoft.Extensions.Logging;
using NetworkCommunicator.Api.Interfaces;
using NetworkCommunicator.PingHandlers;
using NetworkCommunicator.ShutdownHandlers;
using NetworkCommunicator.WakeUpHandlers;

namespace PcRemoteControl
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                // Dependency Injections
                .Services
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<MainPage>()
                    .AddSingleton<IWakeUpHandler, DefaultWakeUpHandler>()
                    .AddSingleton<IShutdownHandler, DefaultShutdownHandler>()
                    .AddSingleton<IPingHandler, DefaultPingHandler>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
