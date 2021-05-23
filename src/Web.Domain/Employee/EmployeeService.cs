using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MySystem.Shared.Library.Dbos;

namespace MySystem.Web.Api.Domain.Employee
{
    public class EmployeeService : IUserService
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository ?? throw new ArgumentNullException();
        }

        public Task<Guid?> GetSessionIdAsync(string username, string password, string device)
        {
            return employeeRepository.GetSessionIdAsync(username, password, device);
        }

        public Task<IEnumerable<Claim>> GetUserClaims(Guid userid)
        {
            return employeeRepository.GetUserClaims(userid);
        }

        public Task<Guid?> GetUserIdAsync(Guid sessionid)
        {
            return GetUserIdAsync(sessionid);
        }

        public Task<IEnumerable<Guid>> GetUserOfficesAsync(Guid userid)
        {
            return GetUserOfficesAsync(userid);
        }

        public Task TerminateSessionAsync(Guid session)
        {
            return TerminateSessionAsync(session);
        }
    }
}
