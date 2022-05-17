using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Essential;

public interface IAdminOperation
{
    Task<IResponseDto> GetListOfUsers();
    Task<IResponseDto> GetListOfTerritories();
    Task<IResponseDto> GetListOfUserClaims();
    Task<IResponseDto> GetListOfUserSessions(bool activeOnly = false);
    Task<IResponseDto> GetListOfUserLogs();

    Task<IResponseDto> UpdateUser(UserDto userDto);
    Task<IResponseDto> AddTerritory(string parentCode, string territoryCode, string territoryName, string countryCode);
    Task<IResponseDto> EditTerritory(string territoryCode);
}