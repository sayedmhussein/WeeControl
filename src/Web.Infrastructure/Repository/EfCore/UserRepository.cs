using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;
using MySystem.Web.Api.Domain.User;
using MySystem.Web.EfRepository;
using MySystem.Web.EfRepository.Models.People;

namespace Web.Infrastructure.Repository.EfCore
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DbContextOptionsBuilder<DataContext> optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=dbdatax;Username=sayed;Include Error Detail = true");

            context = new DataContext(optionsBuilder.Options);
        }

        public async Task<Guid?> CreateOrGetSessionAsync(string username, string device)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Username == username);
            if (employee != null)
            {
                var session = await context.EmployeeSessions.FirstOrDefaultAsync(e => e.Employee == employee && e.DeviceId == device && e.TerminationTs == null);
                if (session == null)
                {
                    var session_ = new EmployeeSessionDbo() { Employee = employee, DeviceId = device };
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

        public async Task LogToSessionAsync(Guid session, string arg)
        {
            var activity = new SessionActivity();
            await context.SessionActivities.AddAsync(activity);
            await context.SaveChangesAsync();
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

        public async Task<Guid?> GetUserId(string username, string password)
        {
            var user = await context.Employees.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            return user?.Id;
        }

        //public async Task<IEnumerable<(DateTime dateTime, string arg)>> GetSessionLogAsync(Guid id, DateTime? from = null, DateTime? until = null)
        //{
        //    var query = context.SessionActivities.Where(x => x.Id == id).AsQueryable();

        //    if (from != null)
        //    {
        //        query.Where(x => x.ActivityTs > from).AsQueryable();
        //    }

        //    if (until != null)
        //    {
        //        query.Where(x => x.ActivityTs < until).AsQueryable();
        //    }

        //    var list = new List<(DateTime datetime, string arg)>();

        //    query.ToList().ForEach(x => list.Add(new (x.ActivityTs, x.Details)));

        //    return list;
        //}
    }
}
