using NetworkCommunicator;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PcRemoteControl
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel;
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        private async void OnWakeUpClicked(object sender, EventArgs e)
        {
            NetworkDetail selectedItem = (NetworkDetail)deviceList.SelectedItem;
            if (selectedItem == null)
                return;

            if (await DisplayAlert("Wake Up Device", $"Are you sure you wish to wake up {selectedItem.Name}", "Wake Up", "Cancel"))
                await WakeOnLan.Wake(selectedItem.MacAddress);
        }

        private async void OnShutdownClicked(object sender, EventArgs e)
        {
            NetworkDetail selectedItem = (NetworkDetail)deviceList.SelectedItem;
            if (selectedItem == null)
                return;

            if (await DisplayAlert("Shutdown Device", $"Are you sure you wish to shutdown {selectedItem.Name}", "Shutdown", "Cancel"))
                ShutdownOnLan.Shutdown(selectedItem.IpAddress);
        }

        private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel((NetworkDetail)deviceList.SelectedItem, true)));
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Device", $"Are you sure you wish to remove {((NetworkDetail)deviceList.SelectedItem).Name}", "Delete", "Cancel"))
                _viewModel.RemoveDevice(deviceList.SelectedItem as NetworkDetail);
        }

        private async void AddDeviceBtn_Clicked(object sender, EventArgs e)
        {
            _viewModel.AddDevice();
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel(_viewModel.NetworkDetails.Last(), false)));
        }
    }
}
