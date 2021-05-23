using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;
using MySystem.Web.Api.Domain.Employee;
using MySystem.Web.EfRepository;
using MySystem.Web.EfRepository.Models.People;

namespace Web.Infrastructure.Repository.EfCore
{
    public class UserRepository : EntityRepository<EmployeeDbo>, IEmployeeRepository<EmployeeDbo>
    {
        private readonly DataContext context;

        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<Guid?> GetSessionIdAsync(string username, string password, string device)
        {
            var user = await context.Employees.FirstOrDefaultAsync(x => x.Username == username && x.Password == password && x.AccountLockArgument == null);
            if (user != null)
            {
                var session = await context.EmployeeSessions.FirstOrDefaultAsync(e => e.Employee == user && e.DeviceId == device && e.TerminationTs == null);
                if (session == null)
                {
                    var session_ = new EmployeeSessionDbo() { Employee = user, DeviceId = device };
                    await context.EmployeeSessions.AddAsync(session_);
                    await context.SaveChangesAsync();
                    return session_.Id;
                }
                else
                {
                    return session?.Id;
                }
            }

            return null;
        }

        public async Task<bool> TerminateSessionAsync(Guid id)
        {
            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == id);
            if (session != null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
