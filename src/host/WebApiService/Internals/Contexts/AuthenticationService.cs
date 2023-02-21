using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.SharedKernel;
using WeeControl.Host.WebApiService.Contexts.User;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Contexts;

internal class AuthenticationService : IAuthenticationService
{
    private readonly IGui gui;
    private readonly IServerOperation server;
    private readonly IDeviceSecurity security;

    public AuthenticationService(IGui gui, IServerOperation server, IDeviceSecurity security)
    {
        this.gui = gui;
        this.server = server;
        this.security = security;
    }
    
    public async Task Login(LoginRequestDto dto)
    {
        var errors = dto.GetModelValidationError();
        if (errors.Any())
        {
            await gui.DisplayAlert(errors.Keys.First());
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post, new Version("1.0"), dto, ControllerApi.Authorization.Route);

        if (response.IsSuccessStatusCode)
        {
            var token = await server.ReadFromContent<TokenResponseDto>(response.Content);
            if (token?.Token is not null)
            {
                await security.UpdateToken(token.Token);
                await gui.NavigateToAsync(ApplicationPages.Essential.OtpPage);
                return;
            }
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                await gui.DisplayAlert("Ínvalid credentials, please try again.");
                break;
            case HttpStatusCode.Forbidden:
                await gui.DisplayAlert("Account is locked, please contact your admin.");
                break;
            default:
                await gui.DisplayAlert("Unexpected Error, please try again.");
                break;
        }
    }

    public async Task UpdateToken()
    {
        if (await security.IsAuthenticated())
        {
            await server.RefreshToken();
            return;
        }

        await gui.DisplayAlert("Please login.");
        await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage);
    }
    
    public async Task UpdateToken(string otp)
    {
        if (await security.IsAuthenticated())
        {
            var response = await server
                .GetResponseMessage(HttpMethod.Put, new Version("1.0"), new object(), ControllerApi.Authorization.Route, 
                    query: new []{"otp", otp});

            if (response.IsSuccessStatusCode)
            {
                var token = await server.ReadFromContent<TokenResponseDto>(response.Content);
                if (token?.Token is not null)
                {
                    await security.UpdateToken(token.Token);
                    await gui.NavigateToAsync(ApplicationPages.Essential.HomePage);
                    return;
                }
            }
        }
        
        await gui.DisplayAlert("Please login.");
        await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage);
    }

    public async Task Logout()
    {
        var response = await server
            .GetResponseMessage(HttpMethod.Delete, new Version("1.0"), ControllerApi.Authorization.Route);

        await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage);
    }
}