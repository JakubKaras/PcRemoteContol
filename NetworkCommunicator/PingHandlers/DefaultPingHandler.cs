using NetworkCommunicator.Api.Entities;
using NetworkCommunicator.Api.Enums;
using NetworkCommunicator.Api.Interfaces;
using System.Net.NetworkInformation;

namespace NetworkCommunicator.PingHandlers
{
    internal class DefaultPingHandler : IPingHandler
    {
        public async Task<bool> Ping(NetworkDetail networkDetail)
        {
            Ping pinger = new();
            var isOnline = false;

            try
            {
                networkDetail.Status = DeviceStatus.Loading;
                var reply = await pinger.SendPingAsync(networkDetail.IpAddress);
                isOnline = reply.Status == IPStatus.Success;
            }
            finally
            {
                networkDetail.Status = isOnline ? DeviceStatus.Online : DeviceStatus.Offline;
                pinger?.Dispose();
            }


            return isOnline;
        }
    }
}
