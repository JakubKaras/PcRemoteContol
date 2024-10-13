using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetworkCommunicator;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace PcRemoteControl
{
    [XmlRoot("MainViewModel")]
    public partial class MainViewModel : ObservableObject
    {
        [XmlIgnore]
        public static readonly string SavePath = Path.Combine(AppContext.BaseDirectory, "MainViewModel.xml");

        [XmlArray("NetworkDetails")]
        public ObservableCollection<NetworkDetail> NetworkDetails { get; set; }

        [XmlIgnore]
        public NetworkDetail? SelectedItem { get; set; }

        [XmlIgnore]
        [ObservableProperty]
        bool isRefreshing;

        public MainViewModel()
        {
            NetworkDetails = new ObservableCollection<NetworkDetail>();
            SelectedItem = NetworkDetails.FirstOrDefault();
        }

        public void AddDevice()
        {
            NetworkDetails.Add(new NetworkDetail());
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

        [RelayCommand]
        private async Task Refresh()
        {
            if (NetworkDetails == null)
            {
                IsRefreshing = false;
                return;
            }

            IsRefreshing = true;
            var tasks = NetworkDetails.Select(x => x.Ping());
            IsRefreshing = false;

            await Task.WhenAll(tasks);
        }
    }
}
