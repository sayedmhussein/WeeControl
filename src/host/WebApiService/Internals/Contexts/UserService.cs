using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.SharedKernel.Contexts.User;
using WeeControl.Host.WebApiService.Contexts.User;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Contexts;

internal class UserService : IUserService
{
    private readonly IServerOperation server;
    private readonly IGui gui;
    private HomeResponseDto? dto;

    public UserService(IServerOperation server, IGui gui)
    {
        this.server = server;
        this.gui = gui;
    }
    
    public async Task<bool> Refresh()
    {
        var response = await server
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), ControllerApi.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var serverDto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            if (serverDto is not null)
            {
                dto = serverDto;
                return true;
            }
        }

        await gui.DisplayAlert("Unexpected error occured when communicating with server!");
        return false;
    }

    public async Task<IEnumerable<HomeNotificationModel>> GetNotifications()
    {
        if (dto == null)
            await Refresh();

        return dto?.Notifications ?? new List<HomeNotificationModel>();
    }

    public async Task<string> GetFullName()
    {
        if (dto == null)
            await Refresh();

        return dto?.FullName ?? string.Empty;
    }
}