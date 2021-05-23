using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MySystem.Web.Api.Domain.User
{
    public interface IUserService
    {
        Task<Guid?> GetUserSessionAsync(string username, string password, string device);

        Task<IEnumerable<Claim>> GetUserClaims(Guid session);

        Task<bool> SessionIsValidAsync(Guid session);

        Task TerminateSessionAsync(Guid session);

        Task LogToSession(Guid session, string str);
    }
}
