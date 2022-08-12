using System.Net;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;

namespace WeeControl.Frontend.ApplicationService.Essential.Legacy;

[Obsolete("Use AuthorizationViewModel Function to logout.")]
public class LogoutLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;

    public string GoodbyMessage => "Good By :)";
    
    public LogoutLegacyViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task LogoutAsync()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Authorization)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete,
        };

        var response = await SendMessageAsync(message, new object());

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.Unauthorized:
                await device.Navigation.NavigateToAsync(Pages.Essential.Authentication.LoginPage, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        await device.Security.DeleteTokenAsync();
    }
}