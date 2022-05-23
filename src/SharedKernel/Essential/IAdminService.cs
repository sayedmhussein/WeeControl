using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.SharedKernel.Essential;

public interface IAdminService
{
    Task<IEnumerable<TerritoryDto>> GetListOfTerritories();
    Task AddTerritory(TerritoryDto territory);
    Task EditTerritory(TerritoryDto territory);
    
    Task<IEnumerable<UserDto>> GetListOfUsers();
    Task<UserDto> GetUserDetails(string username);
    Task UpdateUser(UserDto userDto);
}