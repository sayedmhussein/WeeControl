using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;
using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class PasswordChangeViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    public PasswordChangeModel Model { get; }
    
    public PasswordChangeViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
        Model = new PasswordChangeModel();
    }

    public async Task ChangeMyPassword()
    {
        if (string.IsNullOrWhiteSpace(Model.OldPassword) ||
            string.IsNullOrWhiteSpace(Model.NewPassword) ||
            string.IsNullOrWhiteSpace(Model.ConfirmPassword) ||
            Model.NewPassword != Model.ConfirmPassword)
        {
            await device.Alert.DisplayAlert("Invalid Properties");
            return;
        }

        IsLoading = true;
        await ProcessChangingPassword(SetNewPasswordDtoV1.Create(Model.OldPassword, Model.NewPassword));
        IsLoading = false;
    }

    private async Task ProcessChangingPassword(SetNewPasswordDtoV1? dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.ResetPasswordEndPoint)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
        };
        
        var response = await server.Send(message, dto);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Alert.DisplayAlert("PasswordUpdatedSuccessfully");
                await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage);
                break;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert("InvalidPassword");
                break;
            default:
                await device.Alert.DisplayAlert("DeveloperMinorBug");
                break;
        }
    }
}