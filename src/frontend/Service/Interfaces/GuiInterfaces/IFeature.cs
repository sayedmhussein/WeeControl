namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

public interface IFeature
{
    Task<bool> IsEnergySavingMode();
    Task<string> GetDeviceId();

    Task<(double? Latitude, double? Longitude, double? Elevation)> GetDeviceLocation(bool accurate = false);
    Task<bool> IsMockedDeviceLocation();
}