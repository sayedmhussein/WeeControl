using System.Net;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.Services;

internal class AdminService : IAdminService
{
    private readonly IDevice device;
    private readonly IServerService server;

    public AdminService(IDevice device, IServerService server)
    {
        this.device = device;
        this.server = server;
    }
    
    public async Task<IEnumerable<TerritoryDto>> GetListOfTerritories()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Admin.Territory)),
            Version = new Version("1.0"),
            Method = HttpMethod.Get,
        };

        var response = await server.SendMessageAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var dto = await server.GetObjectFromJsonResponseAsync<ResponseDto<IEnumerable<TerritoryDto>>>(response);
                return dto?.Payload ?? Array.Empty<TerritoryDto>();
            case HttpStatusCode.Conflict:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                return Array.Empty<TerritoryDto>();
            default:
                return Array.Empty<TerritoryDto>();
        }
    }

    public Task AddTerritory(TerritoryDto territory)
    {
        throw new NotImplementedException();
    }

    public Task EditTerritory(TerritoryDto territory)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<UserDto>> GetListOfUsers()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Admin.User)),
            Version = new Version("1.0"),
            Method = HttpMethod.Get,
        };

        var response = await server.SendMessageAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var dto = await server.GetObjectFromJsonResponseAsync<ResponseDto<IEnumerable<UserDto>>>(response);
                return dto?.Payload ?? Array.Empty<UserDto>();
            case HttpStatusCode.Conflict:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                return Array.Empty<UserDto>();
            default:
                return Array.Empty<UserDto>();
        }
    }

    public Task<UserDto> GetUserDetails(string username)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(UserDto userDto)
    {
        throw new NotImplementedException();
    }
}