using NetworkCommunicator;
using NetworkCommunicator.Api.Interfaces;
using System.Xml.Serialization;

namespace PcRemoteControl
{
    public partial class App : Application
    {
        public App(MainViewModel model, IPingHandler pingHandler, IWakeUpHandler wakeUpHandler, IShutdownHandler shutdownHandler)
        {
            InitializeComponent();

            XmlSerializer xs = new XmlSerializer(typeof(MainViewModel));
            MainViewModel? loadedModel = null;

            try
            {
                using (var sr = new StreamReader(MainViewModel.SavePath))
                {
                    loadedModel = (MainViewModel?)xs.Deserialize(sr);
                }

                if (loadedModel == null)
                    throw new FileLoadException("The XML was not loaded correctly.");

                model.NetworkDetails = loadedModel.NetworkDetails;
            }
            catch
            {
                File.Delete(MainViewModel.SavePath);
            }
            finally
            {
                MainPage = new NavigationPage(new MainPage(model, pingHandler, wakeUpHandler, shutdownHandler));
            }
        }
    }
}
