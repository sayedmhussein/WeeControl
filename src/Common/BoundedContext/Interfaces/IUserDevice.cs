using System;
namespace WeeControl.Common.BoundedContext.Interfaces
{
    public interface IUserDevice
    {
        public string DeviceId { get; set; }

        public DateTime TimeStamp { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
