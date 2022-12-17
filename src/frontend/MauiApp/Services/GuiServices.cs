using System.Globalization;
using WeeControl.Frontend.AppService;

namespace WeeControl.Frontend.MauiApp.Services;

public class GuiServices : IDeviceData
{
    public string ServerUrl => "https://localhost:5001/";
    public HttpClient HttpClient { get; set; } = new();

    public Task SendAnEmail(IEnumerable<string> to, string subject, string body)
    {
        return SendAnEmail(to, subject, body, null);
    }

    public Task SendAnEmail(IEnumerable<string> to, string subject, string body, IEnumerable<string> attachments)
    {
        var message = new EmailMessage
        {
            Subject = subject,
            Body = body,
            BodyFormat = EmailBodyFormat.PlainText,
            To = new List<string>(to)
        };
        
        if (attachments != null && attachments.Any())
        {
            foreach (var path in attachments)
            {
                message.Attachments?.Add(new EmailAttachment(path));
            }
        }

        return Email.Default.ComposeAsync(message);
    }

    public Task SendSms(IEnumerable<string> to, string text)
    {
        var message = new SmsMessage(text, to);

        return Sms.Default.ComposeAsync(message);
    }

    public Task<bool> IsConnectedToInternet()
    {
        var accessType = Connectivity.Current.NetworkAccess;
        return Task.FromResult(accessType == NetworkAccess.Internet);
    }

    public Task<bool> PhoneDial(string phoneNo)
    {
        if (PhoneDialer.Default.IsSupported)
        {
            PhoneDialer.Default.Open("000-000-0000");
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

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

    public async Task<(double? Latitude, double? Longitude, double? Elevation)> GetDeviceLocation(bool accurate = false)
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return (location.Latitude, location.Longitude, location.Altitude);
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

        return (null, null, null);
    }

    public async Task<bool> IsMockedDeviceLocation()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Medium);
        var location = await Geolocation.Default.GetLocationAsync(request);

        if (location != null && location.IsFromMockProvider)
        {
            return true;
        }

        return false;
    }

    public Task DisplayAlert(string message)
    {
        return App.Current.MainPage.DisplayAlert("Alert", message, "OK");
    }

    public Task NavigateToAsync(string pageName, bool forceLoad = false)
    {
        return Shell.Current.GoToAsync("//" + pageName);
    }

    public Task Speak(string message)
    {
        return TextToSpeech.Default.SpeakAsync(message);
    }

    public Task CopyToClipboard(string text)
    {
        return Clipboard.Default.SetTextAsync(text);
    }

    public Task<string> ReadFromClipboard()
    {
        return Clipboard.Default.GetTextAsync();
    }

    public Task ClearClipboard()
    {
        return CopyToClipboard(null);
    }

    public string CashDirectory => FileSystem.CacheDirectory;
    public string AppDataDirectory => FileSystem.AppDataDirectory;
    public Task SaveAsync(string key, string value)
    {
        return SecureStorage.Default.SetAsync(key, value);
    }

    public Task<string> GetAsync(string key)
    {
        return SecureStorage.Default.GetAsync(key);
    }

    public Task ClearAsync()
    {
        SecureStorage.Default.RemoveAll();
        return Task.CompletedTask;
    }
}