using System.Net;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Contexts.Authentication;
using WeeControl.Frontend.AppService.Internals.Interfaces;

namespace WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

internal class AuthorizationService : IAuthorizationService
{
    private readonly IDeviceData device;
    private readonly IDeviceSecurity security;
    private readonly IServerOperation serverOperation;
    private const string Route = ApiRouting.AuthorizationRoute;

    public AuthorizationService(IDeviceData device, IDeviceSecurity security, IServerOperation serverOperation)
    {
        this.device = device;
        this.security = security;
        this.serverOperation = serverOperation;
    }

    public async Task<bool> Login(string usernameOrEmail, string password)
    {
        if (string.IsNullOrWhiteSpace(usernameOrEmail) || usernameOrEmail.Trim().Length < 4)
        {
            await device.DisplayAlert(this.InvalidUsernameMessage);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            await device.DisplayAlert(this.InvalidPasswordMessage);
            return false;
        }

        return await ProcessLoginCommand(usernameOrEmail, password);
    }

    public async Task<bool> UpdateToken(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp) || otp.Trim().Length < 4)
        {
            await device.DisplayAlert(InvalidOtpMessage);
            return false;
        }
        
        var response = await serverOperation
            .GetResponseMessage(
                HttpMethod.Put, 
                new Version("1.0"), 
                Route,
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

                await device.NavigateToAsync(ApplicationPages.SplashPage);
                return true;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert(this.InvalidOtpMessage);
                break;
            case HttpStatusCode.Unauthorized:
                await device.DisplayAlert(this.UserIsBlocked);
                await device.NavigateToAsync(ApplicationPages.Essential.UserPage);
                await security.DeleteTokenAsync();
                break;
            default:
                await device.DisplayAlert(ApplicationError);
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
                await device.DisplayAlert(ApplicationError);
                break;
        }
        
        return false;
    }

    public string UsernameLabel => "Username";
    public string PasswordLabel => "Password";
    public string LoginButtonLabel => "Login";
    public string InvalidUsernameMessage => "Please enter valid username.";
    public string InvalidPasswordMessage => "Please enter valid username.";
    public string InvalidOtpMessage => "Please enter valid OTP.";
    public string UnmatchedUsernameAndPassword => "Either username or password are not correct.";
    public string UserIsBlocked => "Account suspended, please contact the admin.";
    public string ApplicationError  => "Internal App Error.";

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
                    await device.SaveAsync(nameof(TokenResponseDto.FullName), responseDto?.FullName ?? string.Empty);
                    if (true) //await server.IsTokenValid())
                    {
                        await device.NavigateToAsync(ApplicationPages.Essential.OtpPage);
                        return true;
                    }
                }
                await device.DisplayAlert(ApplicationError);
                break;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert(UnmatchedUsernameAndPassword);
                break;
            case HttpStatusCode.Forbidden:
                await device.DisplayAlert(UserIsBlocked);
                break;
            default:
                await device.DisplayAlert(ApplicationError + response.StatusCode);
                var c = await response.Content.ReadAsStringAsync();
                Console.WriteLine(c);
                break;
        }

        return false;
    }
}