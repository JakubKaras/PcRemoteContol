using CommunityToolkit.Mvvm.ComponentModel;
using System.Xml.Serialization;

namespace NetworkCommunicator
{
    [Serializable]
    public partial class NetworkDetail : ObservableObject
    {
        [XmlElement]
        [ObservableProperty]
        private string _name;

        [XmlElement]
        [ObservableProperty]
        private string _ipAddress;

        [XmlElement]
        [ObservableProperty]
        private string _macAddress;

        [XmlIgnore]
        [ObservableProperty]
        private DeviceStatus _status;

        private NetworkDetail(string name, string ipAddress, string macAddress, DeviceStatus status)
        {
            Name = name;
            IpAddress = ipAddress;
            MacAddress = macAddress;
            Status = status;
        }

        public NetworkDetail(string name, string ipAddress, string macAddress) : this(name, ipAddress, macAddress, DeviceStatus.Offline)
        {
        }

        public NetworkDetail()
        {
            Name = string.Empty;
            IpAddress = string.Empty;
            MacAddress = string.Empty;
            Status = DeviceStatus.Offline;
        }
    }
}
