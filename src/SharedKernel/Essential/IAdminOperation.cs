using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Essential;

public interface IAdminOperation
{
    Task<IResponseDto<IEnumerable<UserDto>>> GetListOfUsers();
    Task<IResponseDto> GetListOfTerritories();
    Task<IResponseDto<UserDto>> GetUserDetails(string username);
    Task<IResponseDto> UpdateUser(UserDto userDto);
    Task<IResponseDto> AddTerritory(string parentCode, string territoryCode, string territoryName, string countryCode);
    Task<IResponseDto> EditTerritory(TerritoryDto territory);
}