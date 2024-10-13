using NetworkCommunicator.Api.Enums;
using System.Globalization;

namespace PcRemoteControl.Converters
{
    class DeviceStatusToColorConvert : IValueConverter
    {
        private readonly Dictionary<DeviceStatus, Color> _statusLabelTextColors = new()
        {
            { DeviceStatus.Offline, Color.FromRgba("#FF6262") },
            { DeviceStatus.Online, Color.FromRgba("#74FF5C") },
            { DeviceStatus.Loading, Color.FromRgba("#A5ECFD") },
        };

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return Color.FromRgb(255, 255, 255);
            }

            return _statusLabelTextColors[(DeviceStatus)value];
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
