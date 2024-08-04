using System.ComponentModel;
using System.Xml.Serialization;

namespace NetworkCommunicator
{
    [Serializable]
    public class NetworkDetail : INotifyPropertyChanged
    {
        private string _name = "";
        private string _ipAddress = "";
        private string _macAddress = "";

        [XmlElement]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name"); // reports this property
                }
            }
        }

        [XmlElement]
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged("IpAddress"); // reports this property
                }
            }
        }

        [XmlElement]
        public string MacAddress
        {
            get => _macAddress;
            set
            {
                if (_macAddress != value)
                {
                    _macAddress = value;
                    OnPropertyChanged("MacAddress"); // reports this property
                }
            }
        }

        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        public static NetworkDetail Default => new NetworkDetail("MEDIASERVER", "192.168.0.24", "4C:CC:6A:49:65:53");

        public NetworkDetail()
        {
            Name = "";
            IpAddress = "";
            MacAddress = "";
        }

        public NetworkDetail(string name, string ipAddress, string macAddress)
        {
            Name = name;
            IpAddress = ipAddress;
            MacAddress = macAddress;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
