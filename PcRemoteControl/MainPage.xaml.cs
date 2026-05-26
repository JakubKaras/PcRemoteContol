using NetworkCommunicator.Api.Entities;
using NetworkCommunicator.Api.Interfaces;
using PcRemoteControl.Models;

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
        }

        private async void OnWakeUpClicked(object sender, EventArgs e)
        {
            if (((SwipeItem)sender).BindingContext is not NetworkDetail selectedItem)
                return;

            if (await DisplayAlertAsync("Wake Up Device", $"Are you sure you wish to wake up {selectedItem.Name}", "Wake Up", "Cancel"))
                await _wakeUpHandler.WakeUp(selectedItem);
        }

        private async void OnShutdownClicked(object sender, EventArgs e)
        {
            if (((SwipeItem)sender).BindingContext is not NetworkDetail selectedItem)
                return;

            if (!await _pingHandler.Ping(selectedItem))
            {
                await DisplayAlertAsync("Device Is Offline", $"{selectedItem.Name} is already offline.", "Ok");
                return;
            }

            if (await DisplayAlertAsync("Shutdown Device", $"Are you sure you wish to shutdown {selectedItem.Name}", "Shutdown", "Cancel"))
                _shutdownHandler.Shutdown(selectedItem);
        }

        private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            if (((SwipeItem)sender).BindingContext is not NetworkDetail selectedItem)
                return;

            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel(selectedItem, true)));
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            if (((SwipeItem)sender).BindingContext is not NetworkDetail selectedItem)
                return;

            if (await DisplayAlertAsync("Delete Device", $"Are you sure you wish to remove {selectedItem.Name}", "Delete", "Cancel"))
                ((MainViewModel)BindingContext).NetworkDetailsDatabase.RemoveDevice(selectedItem);
        }

        private async void AddDeviceBtn_Clicked(object sender, EventArgs e)
        {
            MainViewModel vm = (MainViewModel)BindingContext;

            vm.NetworkDetailsDatabase.NetworkDetails!.Add(new NetworkDetail());
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel(vm.NetworkDetailsDatabase.NetworkDetails.Last(), false)));
        }

        private void OnLoaded(object? sender, EventArgs e)
        {
            if (((MainViewModel)BindingContext).NetworkDetailsDatabase.NetworkDetails.Count == 0)
            {
                ((MainViewModel)BindingContext).NetworkDetailsDatabase.LoadDevices();
            }
            else
            {
                ((MainViewModel)BindingContext).NetworkDetailsDatabase.SaveDevices();
            }
        }
    }
}
