namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IDevice
    {
        public ILocalStorage LocalStorage { get; }
        
        string DeviceId { get; }
        
        //string Token { get; set; }
        
        //string FullName { get; set; }
        
        string PhotoUrl { get; set; }
    }
}