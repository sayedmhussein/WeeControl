using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Frontend.AppService.Internals.Interfaces;
using WeeControl.Host.WebApiService.Data;

namespace WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

internal class AuthorizationService : IAuthorizationService
{
    private readonly IDeviceData device;
    private readonly IDeviceSecurity security;
    private readonly IServerOperation serverOperation;
    private readonly IDatabaseService database;
    private const string Route = ApiRouting.AuthorizationRoute;

    public AuthorizationService(IDeviceData device, IDeviceSecurity security, IServerOperation serverOperation, IDatabaseService database)
    {
        this.device = device;
        this.security = security;
        this.serverOperation = serverOperation;
        this.database = database;
    }

    public async Task<bool> Login(string usernameOrEmail, string password)
    {
        if (string.IsNullOrWhiteSpace(usernameOrEmail) || usernameOrEmail.Trim().Length < 4)
        {
            await device.DisplayAlert(GetMessage(IAuthorizationService.Message.InvalidUsername));
            return false;
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            await device.DisplayAlert(GetMessage(IAuthorizationService.Message.InvalidPassword));
            return false;
        }

        return await ProcessLoginCommand(usernameOrEmail, password);
    }

    public async Task<bool> UpdateToken(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp) || otp.Trim().Length < 4)
        {
            await device.DisplayAlert(GetMessage(IAuthorizationService.Message.InvalidOtp));
            return false;
        }

        var response = await serverOperation
            .GetResponseMessage(
                HttpMethod.Put,
                new Version("1.0"),
                new[] { Route, otp },
                otp);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:

                var responseDto = await serverOperation.ReadFromContent<TokenResponseDto>(response.Content);
                var token = responseDto?.Token;
                if (token is not null)
                {
                    await security.UpdateTokenAsync(token);
                }

                await device.NavigateToAsync(ApplicationPages.SplashPage, forceLoad: true);
                return true;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert(GetMessage(IAuthorizationService.Message.InvalidUsernameAndPassword));
                break;
            case HttpStatusCode.Unauthorized:
                await device.DisplayAlert(GetMessage(IAuthorizationService.Message.LockedUser));
                await device.NavigateToAsync(ApplicationPages.AuthenticationPage);
                await security.DeleteTokenAsync();
                break;
            default:
                await device.DisplayAlert("Unhandled error");
                break;
        }

        return false;
    }

    public async Task<bool> Logout()
    {
        var response = await serverOperation
            .GetResponseMessage(
                HttpMethod.Delete,
                new Version("1.0"),
                Route, "To send the dto"
                );

        await security.DeleteTokenAsync();
        await database.ClearAllTables();
        await device.ClearClipboard();
        await device.ClearKeysValues();

        await device.NavigateToAsync(ApplicationPages.SplashPage);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                return true;
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.Unauthorized:
                await device.NavigateToAsync(ApplicationPages.SplashPage, forceLoad: true);
                break;
            default:
                await device.DisplayAlert("Unexpected error occured!!!");
                break;
        }

        return false;
    }

    public string GetLabel(IAuthorizationService.Label label)
    {
        return label switch
        {
            IAuthorizationService.Label.LoginHeader => "Please enter your username and password",
            IAuthorizationService.Label.LoginButton => "Login",
            IAuthorizationService.Label.OtpHeader => "Please enter the OTP",
            IAuthorizationService.Label.OtpButton => "Send",
            IAuthorizationService.Label.Username => "Username",
            IAuthorizationService.Label.Password => "Password",

            _ => throw new ArgumentOutOfRangeException(nameof(label), label, null)
        };
    }

    public string GetMessage(IAuthorizationService.Message message)
    {
        switch (message)
        {
            case IAuthorizationService.Message.InvalidUsername:
                return "Please enter valid username.";
            case IAuthorizationService.Message.InvalidPassword:
                return "Please enter valid username.";
            case IAuthorizationService.Message.InvalidUsernameAndPassword:
                return "Either username or password are not correct.";
            case IAuthorizationService.Message.InvalidOtp:
                return "Please enter valid OTP.";
            case IAuthorizationService.Message.LockedUser:
                return "Account suspended, please contact the admin.";
            default:
                throw new ArgumentOutOfRangeException(nameof(message), message, null);
        }
    }

    private async Task<bool> ProcessLoginCommand(string usernameOrEmail, string password)
    {
        var response = await serverOperation
            .GetResponseMessage(
                HttpMethod.Post,
                new Version("1.0"),
                Route,
                LoginRequestDto.Create(usernameOrEmail, password));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await serverOperation.ReadFromContent<TokenResponseDto>(response.Content);
                var token = responseDto?.Token;
                if (token is not null)
                {
                    await security.UpdateTokenAsync(token);
                    await device.SaveKeyValue(nameof(TokenResponseDto.FullName), responseDto?.FullName ?? string.Empty);
                    if (true) //await server.IsTokenValid())
                    {
                        await device.NavigateToAsync(ApplicationPages.Essential.OtpPage);
                        return true;
                    }
                }
                break;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert(GetMessage(IAuthorizationService.Message.InvalidUsernameAndPassword));
                break;
            case HttpStatusCode.Forbidden:
                await device.DisplayAlert(GetMessage(IAuthorizationService.Message.LockedUser));
                break;
            default:
                await device.DisplayAlert("Unexpected error" + response.StatusCode);
                var c = await response.Content.ReadAsStringAsync();
                Console.WriteLine(c);
                break;
        }

        return false;
    }
}