using NetworkCommunicator;
using System.Collections.ObjectModel;

namespace PcRemoteControl
{
    public class MainViewModel
    {
        public ObservableCollection<NetworkDetail> NetworkDetails { get; set; }

        public NetworkDetail? SelectedItem { get; set; }

        public MainViewModel()
        {
            NetworkDetails = [.. NetworkDetail.LoadDevices()];
            SelectedItem = NetworkDetails.FirstOrDefault();
        }

        public void Refresh()
        {
            NetworkDetail.SaveDevices(NetworkDetails);
            NetworkDetails.Clear();
            NetworkDetails = [.. NetworkDetail.LoadDevices()];
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
            NetworkDetail.SaveDevices(NetworkDetails);
        }
    }
}
