using System.Net;
using WeeControl.Frontend.ApplicationService.Contexts.Customer.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Contexts.Customer.ViewModels;

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
        //await ProcessChangingPassword(UserPasswordChangeRequestDto.Create(Model.OldPassword, Model.NewPassword));
        IsLoading = false;
    }

    private async Task ProcessChangingPassword(UserPasswordChangeRequestDto? dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Customer)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
        };
        
        var response = await server.Send(message, dto);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Alert.DisplayAlert("PasswordUpdatedSuccessfully");
                await device.Navigation.NavigateToAsync(Pages.Anonymous.IndexPage);
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