//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using MySystem.Shared.Library.Dbo.Entity;
//using MySystem.Shared.Library.Definition;

//namespace MySystem.Persistence.Api.Domain.Employee
//{
//    public class EmployeeService : IEmployeeService
//    {
//        private readonly IEmployeeRepository employeeRepository;
//        private readonly ILogger logger;

//        public EmployeeService(IEmployeeRepository employeeRepository)
//        {
//            this.employeeRepository = employeeRepository ?? throw new ArgumentNullException();
//        }

//        public EmployeeService(IEmployeeRepository employeeRepository, ILogger logger)
//            : this(employeeRepository)
//        {
//            this.logger = logger;
//        }

//        public Task DeleteAsync(EmployeeDbo entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<IEnumerable<EmployeeDbo>> FindAllAsync()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<EmployeeDbo> FindAsync(params object[] keyValues)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Guid?> GetSessionIdAsync(string username, string password, string device)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(Guid userid)
//        {
//            var list = new List<Claim>();

//            var claims = await employeeRepository.GetUserClaimsAsync(userid);
//            list.AddRange(claims);

//            var sessionid = await employeeRepository.GetSessionIdAsync(userid);
//            list.Add(new Claim(UserClaim.Session, sessionid.ToString()));
            
//            return list;
//        }

//        public Task<IEnumerable<Claim>> GetUserClaimsBySessionIdAsync(Guid userid)
//        {
//            throw new NotImplementedException();
//        }

//        public Task InsertAsync(EmployeeDbo entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Task TerminateSessionAsync(Guid session)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(EmployeeDbo entity)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
