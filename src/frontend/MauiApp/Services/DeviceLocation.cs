using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

namespace WeeControl.Frontend.MauiApp.Services;

public class DeviceLocation : IDeviceLocation
{
    public Task<(double? Latitude, double? Longitude)> GetAccurateLocationAsync()
    {
        throw new NotImplementedException();
    }

    public Task<(double? Latitude, double? Longitude)> GetLastKnownLocationAsync()
    {
        return GetCachedLocation();
    }
    
    private async Task<(double?, double?)> GetCachedLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return (location.Latitude, location.Longitude); //}, Altitude: {location.Altitude}";
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
        }
        catch (Exception ex)
        {
            // Unable to get location
        }

        return (null, null);
    }
}