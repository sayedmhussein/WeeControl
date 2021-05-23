using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MySystem.Web.Api.Domain.Employee
{
    public interface IUserService
    {
        #region Session
        Task<Guid?> GetSessionIdAsync(string username, string password, string device);

        Task<Guid?> GetUserIdAsync(Guid sessionid);

        Task TerminateSessionAsync(Guid session);
        #endregion

        #region UserId
        Task<IEnumerable<Guid>> GetUserOfficesAsync(Guid userid);
        Task<IEnumerable<Claim>> GetUserClaims(Guid userid);
        #endregion
    }
}
