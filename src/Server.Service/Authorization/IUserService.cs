using System.Threading.Tasks;
using WeeControl.Server.Domain.Authorization.Entities;

namespace Server.Service.Authorization
{
    public interface IUserService
    {
        Task<ServiceResponseEnum> AddNewUserAsync(User user);

        Task<ServiceResponseEnum> AlterUserData(User user);

        Task<(ServiceResponseEnum Response, string Token)> GetToken(User user);
        
        
    }
}