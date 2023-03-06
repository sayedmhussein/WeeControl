using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class UserService : IUserService
{
    private readonly IServerOperation server;
    private readonly IGui gui;
    private readonly IDeviceSecurity security;

    public UserService(IServerOperation server, IGui gui, IDeviceSecurity security)
    {
        this.server = server;
        this.gui = gui;
        this.security = security;
    }
    
    public async Task AddUser(UserProfileDto dto)
    {
        if (!dto.Person.IsValidEntityModel())
        {
            await gui.DisplayAlert(dto.Person.GetFirstValidationError());
            return;
        }
        
        if (!dto.User.IsValidEntityModel())
        {
            await gui.DisplayAlert(dto.User.GetFirstValidationError());
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post, 
                new Version("1.0"), dto,
                ControllerApi.Essentials.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var read = await server.ReadFromContent<TokenResponseDto>(response.Content);
            if (read != null && !string.IsNullOrEmpty(read.Token))
            {
                await security.UpdateToken(read.Token);
                await gui.NavigateToAsync(ApplicationPages.Essential.OtpPage);
                return;
            }
            
            await gui.DisplayAlert($"Unexpected Error:{response.StatusCode}");
            return;
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            await gui.DisplayAlert("Please choose another email or username as what you entered already exist");
            return;
        }

        await gui.DisplayAlert($"Unexpected error: {response.StatusCode}");
    }

    public Task EditUserProfile(UserProfileUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserProfileDto> GetUserProfile()
    {
        throw new NotImplementedException();
    }

    public async Task ChangePassword(UserPasswordChangeRequestDto dto)
    {
        if (dto.IsValidEntityModel() == false)
        {
            await gui.DisplayAlert("invalid data");
            return;
        }
        
        var response = await server
            .GetResponseMessage(HttpMethod.Patch, 
                new Version("1.0"), dto,
                ControllerApi.Essentials.User.Route,
                endpoint:ControllerApi.Essentials.User.PasswordEndpoint);

        if (response.IsSuccessStatusCode)
        {
            await gui.DisplayAlert("Password was changed successfully");
            await gui.NavigateToAsync(ApplicationPages.Essential.HomePage);
            return;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            await gui.DisplayAlert("Old password isn't matching, please try again.");
            return;
        }

        await gui.DisplayAlert($"Unexpected Error {response.StatusCode}");
        throw new ArgumentOutOfRangeException();
    }
}