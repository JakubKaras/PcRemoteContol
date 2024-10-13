using NetworkCommunicator;
using System.Xml.Serialization;

namespace PcRemoteControl
{
    public partial class App : Application
    {
        public App(MainViewModel model)
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
            }
            catch
            {
                File.Delete(MainViewModel.SavePath);
            }
            finally
            {
                MainPage = new NavigationPage(new MainPage(loadedModel ?? model));
            }
        }
    }
}
