using NetworkCommunicator.Api.Entities;

namespace PcRemoteControl.Models
{
    public class AddOrEditDeviceViewModel
    {
        private readonly bool _isEdit = false;

        public NetworkDetail Device { get; init; }

        public string Title { get => _isEdit ? "Editing device" : "Adding new device"; }

        public AddOrEditDeviceViewModel(NetworkDetail device, bool isEdit)
        {
            _isEdit = isEdit;
            Device = device;
        }
    }
}
