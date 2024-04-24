using System.Net;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.ExtensionHelpers;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class PersonService(IGui gui, IServerOperation server, IDeviceSecurity security) : IPersonService
{
    public async Task Register(UserProfileDto dto)
    {
        if (!dto.IsValidEntityModel())
        {
            await gui.DisplayAlert(dto.GetFirstValidationError());
            return;
        }

        if (!dto.IsValidEntityModel())
        {
            await gui.DisplayAlert(dto.GetFirstValidationError());
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post,
                new Version("1.0"), dto,
                ApiRouting.Essentials.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var read = await server.ReadFromContent<TokenResponseDto>(response.Content);
            if (read != null && !string.IsNullOrEmpty(read.Token))
            {
                await security.UpdateToken(read.Token);
                await gui.NavigateTo(ApplicationPages.Essential.OtpPage);
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
    
    public async Task RequestPasswordReset(UserPasswordResetRequestDto dto)
    {
        if (dto.IsValidEntityModel() == false)
        {
            await gui.DisplayAlert("invalid data");
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post,
                new Version("1.0"), dto,
                ApiRouting.Essentials.User.Route,
                ApiRouting.Essentials.User.PasswordEndpoint);

        if (response.IsSuccessStatusCode)
        {
            await gui.DisplayAlert("Please check your inbox for more instructions");
            await gui.NavigateTo(ApplicationPages.Essential.HomePage);
        }
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
                ApiRouting.Essentials.User.Route,
                ApiRouting.Essentials.User.PasswordEndpoint);

        if (response.IsSuccessStatusCode)
        {
            await gui.DisplayAlert("Password was changed successfully");
            await gui.NavigateTo(ApplicationPages.Essential.HomePage);
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