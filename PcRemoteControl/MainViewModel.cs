using NetworkCommunicator;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace PcRemoteControl
{
    [XmlRoot("MainViewModel")]
    public class MainViewModel
    {
        [XmlIgnore]
        public static readonly string SavePath = Path.Combine(AppContext.BaseDirectory, "MainViewModel.xml");

        [XmlArray("NetworkDetails")]
        public ObservableCollection<NetworkDetail> NetworkDetails { get; set; }

        public NetworkDetail? SelectedItem { get; set; }

        public MainViewModel()
        {
            NetworkDetails = new ObservableCollection<NetworkDetail>();
            SelectedItem = NetworkDetails.FirstOrDefault();
        }

        public void AddDevice()
        {
            NetworkDetails.Add(new NetworkDetail("", "", ""));
        }

        public void RemoveDevice(NetworkDetail? detail)
        {
            if(detail == null || !NetworkDetails.Contains(detail))
                return;

            NetworkDetails.Remove(detail);
            SaveDevices();
        }

        public void SaveDevices()
        {
            XmlSerializer xs = new XmlSerializer(typeof(MainViewModel));
            TextWriter tw = new StreamWriter(SavePath);
            xs.Serialize(tw, this);
        }
    }
}
