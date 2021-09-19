namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IDevice
    {
        public ILocalStorage LocalStorage { get; }
        
        string DeviceId { get; }

        string PhotoUrl { get; set; }
    }
}