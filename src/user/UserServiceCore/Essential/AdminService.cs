using System.Net;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;
using WeeControl.User.UserServiceCore.InternalHelpers;

namespace WeeControl.User.UserServiceCore.Essential;

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
                return dto.Payload;
            case HttpStatusCode.Conflict:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                return null;
            default:
                return null;
        }
    }

    public Task<IResponseDto> AddTerritory(TerritoryDto territory)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseDto> EditTerritory(TerritoryDto territory)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseDto<IEnumerable<UserDto>>> GetListOfUsers()
    {
        throw new NotImplementedException();
    }

    public Task<IResponseDto<UserDto>> GetUserDetails(string username)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseDto> UpdateUser(UserDto userDto)
    {
        throw new NotImplementedException();
    }
}