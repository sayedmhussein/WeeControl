using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbo;
using MySystem.Web.Api.Domain.Employee;
using MySystem.Web.EfRepository;

namespace MySystem.Web.Infrastructure.Repository.Employee
{
    public class EmployeeEfCore : IEmployeeRepository
    {
        private readonly DataContext context;

        public EmployeeEfCore(DbContextOptions options)
            : this(new DataContext((DbContextOptions<DataContext>)options))
        {
        }

        public EmployeeEfCore(DataContext context)
        {
            this.context = context ?? throw new ArgumentNullException();
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

        public async Task<Guid?> GetUserIdAsync(Guid sessionid)
        {
            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == sessionid && x.TerminationTs == null);
            return session?.EmployeeId;
        }

        public async Task TerminateSessionAsync(Guid sessionid)
        {
            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == sessionid);
            if (session != null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Guid>> GetUserOfficesAsync(Guid userid)
        {
            var office = await  context.Employees.FirstOrDefaultAsync(x => x.Id == userid);
            var offices = await context.Offices.Include(x => x.Parent).ToListAsync();
            var list = new List<Guid>();
            offices.ForEach(x => list.Add(x.Id));
            return list;
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(Guid userid)
        {
            var list = new List<Claim>();

            var claims = await context.EmployeeClaims.Where(x => x.EmployeeId == userid && x.RevokedTs == null).ToListAsync();
            claims.ForEach(x => list.Add(new Claim(x.ClaimType, x.ClaimValue)));

            return list;
        }

        public async Task<Guid?> GetSessionIdAsync(Guid employeeid)
        {
            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.EmployeeId == employeeid && x.TerminationTs == null);
            return session?.Id;
        }

        public Task<EmployeeDbo> FindAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeDbo>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(EmployeeDbo entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(EmployeeDbo entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(EmployeeDbo entity)
        {
            throw new NotImplementedException();
        }
    }
}
