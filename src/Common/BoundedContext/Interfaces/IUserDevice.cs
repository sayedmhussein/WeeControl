using System;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Common.BoundedContext.Interfaces
{
    public interface IUserDevice : IClientDevice
    {
        public string DeviceId { get; }

        public DateTime TimeStamp { get; }

        public double? Latitude { get; }

        public double? Longitude { get; }

        public string ServerBaseAddress { get; set; }
    }
}
