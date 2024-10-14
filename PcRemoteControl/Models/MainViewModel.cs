using CommunityToolkit.Mvvm.ComponentModel;
using NetworkCommunicator.Api.Entities;
using NetworkCommunicator.Api.Interfaces;
using PcRemoteControl.Entities;
using System.Windows.Input;

namespace PcRemoteControl.Models
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPingHandler? _pingHandler;

        public NetworkDetailsDatabase NetworkDetailsDatabase { get; set; }

        public NetworkDetail? SelectedItem { get; set; }

        [ObservableProperty]
        bool isRefreshing;

        public ICommand? RefreshCommand { get; set; }

        public MainViewModel(NetworkDetailsDatabase networkDetailsDatabase, IPingHandler pingHandler)
        {
            _pingHandler = pingHandler;

            NetworkDetailsDatabase = networkDetailsDatabase;
            SelectedItem = NetworkDetailsDatabase.NetworkDetails.FirstOrDefault();

            RefreshCommand = new Command(async () => await Refresh());
        }

        private async Task Refresh()
        {
            if (NetworkDetailsDatabase.NetworkDetails == null || _pingHandler == null)
            {
                IsRefreshing = false;
                return;
            }

            IsRefreshing = true;
            var tasks = NetworkDetailsDatabase.NetworkDetails
                .Where(x  => !string.IsNullOrEmpty(x.IpAddress))
                .Select(_pingHandler.Ping);
            IsRefreshing = false;

            await Task.WhenAll(tasks);
        }
    }
}
