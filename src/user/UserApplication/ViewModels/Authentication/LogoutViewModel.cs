using System.Net;
using WeeControl.SharedKernel;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Authentication;

public class LogoutViewModel : ViewModelBase
{
    private readonly IDevice device;

    public string GoodbyMessage => "Good By :)";
    
    public LogoutViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task LogoutAsync()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Root)),
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
                await device.Navigation.NavigateToAsync(Pages.Authentication.LoginPage, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        await device.Security.DeleteTokenAsync();
    }
}