﻿using NetworkCommunicator;

namespace PcRemoteControl
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
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
                await selectedItem.Wake();
        }

        private async void OnShutdownClicked(object sender, EventArgs e)
        {
            NetworkDetail selectedItem = (NetworkDetail)deviceList.SelectedItem;
            if (selectedItem == null)
                return;

            if (!await selectedItem.Ping())
            {
                await DisplayAlert("Device Is Offline", $"{selectedItem.Name} is already offline.", "Ok");
                return;
            }

            if (await DisplayAlert("Shutdown Device", $"Are you sure you wish to shutdown {selectedItem.Name}", "Shutdown", "Cancel"))
                selectedItem.Shutdown();
        }

        private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel((NetworkDetail)deviceList.SelectedItem, true)));
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Device", $"Are you sure you wish to remove {((NetworkDetail)deviceList.SelectedItem).Name}", "Delete", "Cancel"))
                ((MainViewModel)BindingContext).RemoveDevice(deviceList.SelectedItem as NetworkDetail);
        }

        private async void AddDeviceBtn_Clicked(object sender, EventArgs e)
        {
            ((MainViewModel)BindingContext).AddDevice();
            await Navigation.PushAsync(new AddOrEditDevicePage(new AddOrEditDeviceViewModel(((MainViewModel)BindingContext).NetworkDetails.Last(), false)));
        }

        private void OnLoaded(object? sender, EventArgs e)
        {
            ((MainViewModel)BindingContext).SaveDevices();
        }
    }
}
