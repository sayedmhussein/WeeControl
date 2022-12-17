using System.Net;
using System.Net.Http.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Contexts.Authentication;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService.Contexts.Home;
using WeeControl.Frontend.AppService.Internals.Interfaces;

namespace WeeControl.Frontend.AppService.Contexts.Authentication;

internal class AuthorizationService : IAuthorizationService
{
    public AuthorizationGui GuiStrings { get; }
    
    private readonly IDeviceData device;
    private readonly IDeviceSecurity security;
    private readonly IServerOperation server;
    private const string Route = ApiRouting.AuthorizationRoute;

    public AuthorizationService(IDeviceData device, IDeviceSecurity security, IServerOperation server)
    {
        this.device = device;
        this.security = security;
        this.server = server;

        GuiStrings = new AuthorizationGui();
    }

    public async Task<bool> Login(string usernameOrEmail, string password)
    {
        if (string.IsNullOrWhiteSpace(usernameOrEmail) || usernameOrEmail.Trim().Length < 4)
        {
            await device.DisplayAlert(GuiStrings.InvalidUsernameMessage);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            await device.DisplayAlert(GuiStrings.InvalidPasswordMessage);
            return false;
        }

        return await ProcessLoginCommand(usernameOrEmail, password);
    }

    public async Task<bool> UpdateToken(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp) || otp.Trim().Length < 4)
        {
            await device.DisplayAlert(GuiStrings.InvalidOtpMessage);
            return false;
        }

        var response = await server.Send(new HttpRequestMessage
        {
            RequestUri = new Uri(server.GetFullAddress(Route)),
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
                await device.DisplayAlert(GuiStrings.InvalidOtpMessage);
                break;
            case HttpStatusCode.Unauthorized:
                await device.DisplayAlert(GuiStrings.UserIsBlocked);
                await device.NavigateToAsync(ApplicationPages.Essential.UserPage);
                await security.DeleteTokenAsync();
                break;
            default:
                await device.DisplayAlert(GuiStrings.ApplicationError);
                break;
        }

        return false;
    }

    public async Task<bool> Logout()
    {
        var response = await server.Send(new HttpRequestMessage
        {
            RequestUri = new Uri(server.GetFullAddress(Route)),
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
                await device.DisplayAlert(GuiStrings.ApplicationError);
                break;
        }
        
        return false;
    }
    
    private async Task<bool> ProcessLoginCommand(string usernameOrEmail, string password)
    {
        var response = await server.Send(
            new HttpRequestMessage
            {
                RequestUri = new Uri(server.GetFullAddress(Route)),
                Version = new Version("1.0"),
                Method = HttpMethod.Post
            }, 
            LoginRequestDto.Create(usernameOrEmail, password));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await server.ReadFromContent<TokenResponseDto>(response.Content);
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
                await device.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
                break;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert(GuiStrings.UnmatchedUsernameAndPassword);
                break;
            case HttpStatusCode.Forbidden:
                await device.DisplayAlert(GuiStrings.UserIsBlocked);
                break;
            default:
                await device.DisplayAlert(GuiStrings.ApplicationError + response.StatusCode);
                var c = await response.Content.ReadAsStringAsync();
                Console.WriteLine(c);
                break;
        }

        return false;
    }
}