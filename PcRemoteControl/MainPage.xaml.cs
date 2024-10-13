using NetworkCommunicator.Api.Entities;
using NetworkCommunicator.Api.Interfaces;

namespace PcRemoteControl
{
    public partial class MainPage : ContentPage
    {
        private readonly IPingHandler _pingHandler;
        private readonly IWakeUpHandler _wakeUpHandler;
        private readonly IShutdownHandler _shutdownHandler;

        public MainPage(MainViewModel viewModel, IPingHandler pingHandler, IWakeUpHandler wakeUpHandler, IShutdownHandler shutdownHandler)
        {
            _pingHandler = pingHandler;
            _wakeUpHandler = wakeUpHandler;
            _shutdownHandler = shutdownHandler;

            InitializeComponent();
            BindingContext = viewModel;

            Loaded += OnLoaded;
        }

        private async void OnWakeUpClicked(object sender, EventArgs e)
        {
            NetworkDetail selectedItem = (NetworkDetail)deviceList.SelectedItem;
            if (selectedItem == null)
                return;

            if (await DisplayAlert("Wake Up Device", $"Are you sure you wish to wake up {selectedItem.Name}", "Wake Up", "Cancel"))
                await _wakeUpHandler.WakeUp(selectedItem);
        }

        private async void OnShutdownClicked(object sender, EventArgs e)
        {
            NetworkDetail selectedItem = (NetworkDetail)deviceList.SelectedItem;
            if (selectedItem == null)
                return;

            if (!await _pingHandler.Ping(selectedItem))
            {
                await DisplayAlert("Device Is Offline", $"{selectedItem.Name} is already offline.", "Ok");
                return;
            }

            if (await DisplayAlert("Shutdown Device", $"Are you sure you wish to shutdown {selectedItem.Name}", "Shutdown", "Cancel"))
                _shutdownHandler.Shutdown(selectedItem);
        }

        private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel((NetworkDetail)deviceList.SelectedItem, true)));
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            NetworkDetail device = (NetworkDetail)deviceList.SelectedItem;

            if (await DisplayAlert("Delete Device", $"Are you sure you wish to remove {device.Name}", "Delete", "Cancel"))
                ((MainViewModel)BindingContext).RemoveDevice(device);
        }

        private async void AddDeviceBtn_Clicked(object sender, EventArgs e)
        {
            MainViewModel vm = (MainViewModel)BindingContext;

            vm.NetworkDetails.Add(new NetworkDetail());
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel(vm.NetworkDetails.Last(), false)));
        }

        private void OnLoaded(object? sender, EventArgs e)
        {
            ((MainViewModel)BindingContext).SaveDevices();
        }
    }
}
