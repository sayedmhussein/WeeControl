namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IDevice
    {
        string DeviceId { get; }
        
        string Token { get; set; }
        
        string FullName { get; set; }
        
        string PhotoUrl { get; set; }
    }
}