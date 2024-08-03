using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NetworkCommunicator
{
    public partial class NetworkDetail : ObservableObject
    {
        private static string _networkDetailsPath = Path.Combine(AppContext.BaseDirectory, "NetworkDetails.xml");

        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _ipAddress;
        [ObservableProperty]
        private string _macAddress;

        public static NetworkDetail Default => new NetworkDetail("MEDIASERVER", "192.168.0.24", "4C:CC:6A:49:65:53");

        public NetworkDetail(string name, string ipAddress, string macAddress)
        {
            Name = name;
            IpAddress = ipAddress;
            MacAddress = macAddress;
        }

        public string Detail { get => $"\tIP Address: {IpAddress}\n\tMAC Address: {MacAddress}"; }

        public static IEnumerable<NetworkDetail> LoadDevices()
        {
            if (!File.Exists(_networkDetailsPath))
            {
                return Enumerable.Empty<NetworkDetail>().Append(Default);
            }
            XElement networkDetails = XElement.Load(_networkDetailsPath);

            var list = networkDetails.Descendants("NetworkDetail")
                .Select(GetXmlElementValues)
                .Append(Default);
            return list;
        }

        public static void SaveDevices(IEnumerable<NetworkDetail> networkDetails)
        {
            XElement xmlDoc = new XElement("NetworkDetails", networkDetails.Select(detail => detail.ToXml()));

            xmlDoc.Save(_networkDetailsPath);
        }

        private static NetworkDetail GetXmlElementValues(XElement element)
        {
            NetworkDetail detail = new NetworkDetail("", "", "");
            if (element != null)
            {
                foreach (var property in detail.GetType().GetProperties())
                {
                    XElement? item = element.Element(property.Name);
                    if (item != null)
                    {
                        property.SetValue(detail, item.Value);
                    }
                }
            }

            return detail;
        }

        private XElement ToXml()
        {
            return new XElement(
                "NetworkDetail",
                typeof(NetworkDetail).GetProperties().Select(property => new XElement(nameof(property.Name), property.GetValue(this)))
            );
        }
    }
}
