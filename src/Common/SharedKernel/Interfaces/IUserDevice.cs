namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IUserDevice : IUserOperation, IUserCommunication
    {
        public string DeviceId { get; }

        public DateTime TimeStamp { get; }

        public double? Latitude { get; }

        public double? Longitude { get; }
    }
}
