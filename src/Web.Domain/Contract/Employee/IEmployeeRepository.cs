using System;
using System.Threading.Tasks;
using MySystem.Shared.Library.Dbo;
using MySystem.Shared.Library.Dbo.Entity;

namespace MySystem.Web.Api.Domain.Employee
{
    public interface IEmployeeRepository : IRepositoryAsync<EmployeeDbo>, IRepositoryAsync<EmployeeSessionDbo>, IRepositoryAsync<EmployeeClaimDbo>
    {
    }
}
