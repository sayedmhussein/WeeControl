namespace WeeControl.Common.FunctionalService.Interfaces
{
    public interface IUserDevice
    {
        public string DeviceId { get; }

        public DateTime TimeStamp { get; }

        public double? Latitude { get; }

        public double? Longitude { get; }
    }
}
