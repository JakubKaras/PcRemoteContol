using NetworkCommunicator.Api.Entities;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace PcRemoteControl.Entities
{
    [XmlRoot("NetworkDetailsDatabase")]
    public class NetworkDetailsDatabase
    {
        [XmlIgnore]
        private static string SavePath = Path.Combine(AppContext.BaseDirectory, "NetworkDetailsDatabase.xml");

        [XmlIgnore]
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(NetworkDetailsDatabase));

        [XmlArray("NetworkDetails")]
        public ObservableCollection<NetworkDetail> NetworkDetails { get; set; } = new ObservableCollection<NetworkDetail>();

        public void LoadDevices()
        {
            NetworkDetailsDatabase? loadedData;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NetworkDetailsDatabase));
                using (var sr = new StreamReader(SavePath))
                {
                    loadedData = (NetworkDetailsDatabase?)serializer.Deserialize(sr);
                }

                if (loadedData == null)
                    throw new FileLoadException("The XML was not loaded correctly.");

                NetworkDetails = loadedData.NetworkDetails;
            }
            catch
            {
                File.Delete(SavePath);
            }
        }

        public void SaveDevices()
        {
            TextWriter tw = new StreamWriter(SavePath);
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkDetailsDatabase));
            serializer.Serialize(tw, this);
        }

        public void RemoveDevice(NetworkDetail? detail)
        {
            if (detail == null || NetworkDetails == null || !NetworkDetails.Contains(detail))
                return;

            NetworkDetails.Remove(detail);
            SaveDevices();
        }
    }
}
