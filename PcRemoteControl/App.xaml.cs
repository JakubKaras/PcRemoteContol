using NetworkCommunicator.Api.Interfaces;
using PcRemoteControl.Models;

namespace PcRemoteControl
{
    public partial class App : Application
    {
        public App(MainViewModel model, IPingHandler pingHandler, IWakeUpHandler wakeUpHandler, IShutdownHandler shutdownHandler)
        {
            InitializeComponent();

            model.NetworkDetailsDatabase.LoadDevices();
            MainPage = new NavigationPage(new MainPage(model, pingHandler, wakeUpHandler, shutdownHandler));
        }
    }
}
