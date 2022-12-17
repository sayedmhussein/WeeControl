namespace WeeControl.Frontend.AppService.GuiInterfaces.Obsolute;

[Obsolete("User IFeature interface." ,true)]
public interface IDeviceLocation
{
    Task<(double? Latitude, double? Longitude)> GetAccurateLocationAsync();
    Task<(double? Latitude, double? Longitude)> GetLastKnownLocationAsync();
}