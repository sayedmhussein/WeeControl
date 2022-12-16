using System.Net;
using System.Net.Http.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService.AppInterfaces;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

namespace WeeControl.Frontend.AppService.AppServices;

internal class AuthorizationService : IAuthorizationService
{
    private readonly IGuiInterface device;
    private readonly IDeviceSecurity security;
    private readonly IServerOperation server;

    public AuthorizationService(IGuiInterface device, IDeviceSecurity security, IServerOperation server)
    {
        this.device = device;
        this.security = security;
        this.server = server;
    }
    
    public async Task<bool> IsAuthorized()
    {
        return await server.IsTokenValid() && await security.IsAuthenticatedAsync();
    }

    public async Task<bool> Login(string usernameOrEmail, string password)
    {
        if (string.IsNullOrWhiteSpace(usernameOrEmail) || usernameOrEmail.Trim().Length < 4)
        {
            await device.DisplayAlert("Please enter correct username");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            await device.DisplayAlert("Please enter correct password");
            return false;
        }

        return await ProcessLoginCommand(usernameOrEmail, password);
    }

    public async Task<bool> UpdateToken(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp) || otp.Trim().Length < 4)
        {
            await device.DisplayAlert("Invalid OTP, Please try again");
            return false;
        }

        var response = await server.Send(new HttpRequestMessage
        {
            RequestUri = new Uri(server.GetFullAddress(Api.Essential.Authorization.Route)),
            Version = new Version("1.0"),
            Method = string.IsNullOrWhiteSpace(otp) ? HttpMethod.Patch : HttpMethod.Put
        }, otp);
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var responseDto = await server.ReadFromContent<TokenResponseDto>(response.Content);
                //var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
                var token = responseDto?.Token;
                if (token is not null)
                {
                    await security.UpdateTokenAsync(token);
                }

                if (otp is not null)
                {
                    await device.NavigateToAsync(ApplicationPages.Essential.SplashPage);
                }
                return true;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
            case HttpStatusCode.Unauthorized:
                await device.DisplayAlert("AlertEnum.DeveloperMinorBug");
                await device.NavigateToAsync(ApplicationPages.Essential.UserPage);
                await security.DeleteTokenAsync();
                break;
            default:
                await device.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        return false;
    }

    public async Task<bool> Logout()
    {
        var response = await server.Send(new HttpRequestMessage
        {
            RequestUri = new Uri(server.GetFullAddress(Api.Essential.Authorization.Route)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete
        });
        
        await security.DeleteTokenAsync();
        await device.NavigateToAsync(ApplicationPages.Essential.SplashPage);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                return true;
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.Unauthorized:
                await device.NavigateToAsync(ApplicationPages.Essential.SplashPage, forceLoad: true);
                break;
            default:
                await device.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }
        
        return false;
    }
    
    private async Task<bool> ProcessLoginCommand(string usernameOrEmail, string password)
    {
        var response = await server.Send(
            new HttpRequestMessage
            {
                RequestUri = new Uri(server.GetFullAddress(Api.Essential.Authorization.Route)),
                Version = new Version("1.0"),
                Method = HttpMethod.Post
            }, 
            LoginRequestDto.Create(usernameOrEmail, password));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenResponseDto>>();
                var token = responseDto?.Payload?.Token;
                if (token is not null)
                {
                    await security.UpdateTokenAsync(token);
                    await device.SaveAsync(nameof(TokenResponseDto.FullName), responseDto?.Payload?.FullName ?? string.Empty);
                    if (true) //await server.IsTokenValid())
                    {
                        await device.NavigateToAsync(ApplicationPages.Essential.OtpPage);
                        return true;
                    }
                }
                await device.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
                break;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert("Invalid username or password, please try again.");
                break;
            case HttpStatusCode.Forbidden:
                await device.DisplayAlert("Your account has been locked, contact the administrator.");
                break;
            default:
                await device.DisplayAlert("Unexpected error occured! " + response.StatusCode);
                break;
        }

        return false;
    }
}