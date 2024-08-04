using NetworkCommunicator;
using System.Xml.Serialization;

namespace PcRemoteControl
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            XmlSerializer xs = new XmlSerializer(typeof(MainViewModel));
            MainViewModel? model = null;

            try
            {
                using (var sr = new StreamReader(MainViewModel.SavePath))
                {
                    model = (MainViewModel?)xs.Deserialize(sr);
                }

                if (model == null)
                    throw new FileLoadException("The XML was not loaded correctly.");
            }
            catch
            {
                File.Delete(MainViewModel.SavePath);

                model = new MainViewModel();
                model.NetworkDetails.Add(NetworkDetail.Default);
                model.SelectedItem = model.NetworkDetails.First();
            }
            finally
            {
                MainPage = new NavigationPage(new MainPage(model ?? new MainViewModel()));
            }
        }
    }
}
