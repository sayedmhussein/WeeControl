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
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete,
        };

        var response = await SendMessageAsync(message, new object());

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Navigation.NavigateToAsync(Pages.Authentication.Login, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        await device.Security.DeleteTokenAsync();
    }
}