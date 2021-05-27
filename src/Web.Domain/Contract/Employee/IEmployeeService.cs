using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MySystem.Shared.Library.Dbo;
using MySystem.Shared.Library.Dbo.Entity;

namespace MySystem.Web.Api.Domain.Employee
{
    public interface IEmployeeService : IRepositoryAsync<EmployeeDbo>
    {
        #region Authentication Controller
        Task<Guid?> GetSessionIdAsync(string username, string password, string device);
        Task<IEnumerable<Claim>> GetUserClaimsBySessionIdAsync(Guid userid);
        Task TerminateSessionAsync(Guid session);
        #endregion
    }
}
