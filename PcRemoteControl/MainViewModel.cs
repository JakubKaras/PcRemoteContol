using CommunityToolkit.Mvvm.ComponentModel;
using NetworkCommunicator.Api.Entities;
using NetworkCommunicator.Api.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;

namespace PcRemoteControl
{
    [XmlRoot("MainViewModel")]
    public partial class MainViewModel : ObservableObject
    {
        [XmlIgnore]
        public static readonly string SavePath = Path.Combine(AppContext.BaseDirectory, "MainViewModel.xml");

        [XmlIgnore]
        private readonly IPingHandler? _pingHandler;

        [XmlArray("NetworkDetails")]
        public ObservableCollection<NetworkDetail> NetworkDetails { get; set; }

        [XmlIgnore]
        public NetworkDetail? SelectedItem { get; set; }

        [XmlIgnore]
        [ObservableProperty]
        bool isRefreshing;

        [XmlIgnore]
        public ICommand? RefreshCommand { get; set; }

        public MainViewModel()
        {
            NetworkDetails = new ObservableCollection<NetworkDetail>();
            SelectedItem = NetworkDetails.FirstOrDefault();

            RefreshCommand = new Command(async () => await Refresh());
        }

        public MainViewModel(IPingHandler pingHandler) : this()
        {
            _pingHandler = pingHandler;
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

        private async Task Refresh()
        {
            if (NetworkDetails == null || _pingHandler == null)
            {
                IsRefreshing = false;
                return;
            }

            IsRefreshing = true;
            var tasks = NetworkDetails.Select(_pingHandler.Ping);
            IsRefreshing = false;

            await Task.WhenAll(tasks);
        }
    }
}
