using System.Globalization;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.MauiApp.Services;

[Obsolete("Use GuiService class", true)]
public class FeatureService : IFeature
{
    public Task<bool> IsEnergySavingMode()
    {
        return Task.FromResult(Battery.Default.EnergySaverStatus == EnergySaverStatus.On);
    }

    public Task<string> GetDeviceId()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"Model: {DeviceInfo.Current.Model}");
        sb.AppendLine($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
        sb.AppendLine($"Name: {DeviceInfo.Name}");
        sb.AppendLine($"OS Version: {DeviceInfo.VersionString}");
        sb.AppendLine($"Refresh Rate: {DeviceInfo.Current}");
        sb.AppendLine($"Idiom: {DeviceInfo.Current.Idiom}");
        sb.AppendLine($"Platform: {DeviceInfo.Current.Platform}");

        var isVirtual = DeviceInfo.Current.DeviceType switch
        {
            DeviceType.Physical => false,
            DeviceType.Virtual => true,
            _ => false
        };

        sb.AppendLine(new Random().NextDouble().ToString(CultureInfo.InvariantCulture));

        sb.AppendLine($"Virtual device? {isVirtual}");
        
        return Task.FromResult<string>(sb.ToString());
    }

    public Task<(double? Latitude, double? Longitude, double? Elevation)> GetDeviceLocation(bool accurate = false)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsMockedDeviceLocation()
    {
        GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
        Location location = await Geolocation.Default.GetLocationAsync(request);

        if (location != null && location.IsFromMockProvider)
        {
            return true;
        }

        return false;
    }
}