using WeeControl.Presentations.ServiceLibrary.Interfaces;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Presentations.ServiceLibrary.EssentialContext;

public class AdminOperation : OperationBase, IAdminOperation
{
    private readonly IDevice device;
    private readonly IDeviceAlert alert;

    public AdminOperation(IDevice device, IDeviceAlert alert) : base(device)
    {
        this.device = device;
        this.alert = alert;
    }
    
    public async Task<IResponseDto<IEnumerable<TerritoryDto>>> GetListOfTerritories()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.DeviceServerCommunication.GetFullAddress(Api.Essential.Admin.Territory)),
            Version = new Version("1.0"),
            Method = HttpMethod.Get,
        };

        var response = await SendMessageAsync(message);

        // switch (response.StatusCode)
        // {
        //     case HttpStatusCode.OK:
        //     case HttpStatusCode.Accepted:
        //         var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<TerritoryDto>>>();
        //         
        //         return ResponseToUi.Accepted(response.StatusCode);
        //     case HttpStatusCode.Conflict:
        //         return ResponseToUi.Rejected(response.StatusCode, "Either username or password already exist!");
        //     default:
        //         return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
        //     
        // }

        throw new NotImplementedException();
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