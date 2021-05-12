using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sayed.MySystem.ServerData
{
    public class CustomFunctionV1
    {
        private readonly DataContext context;

        public CustomFunctionV1(DataContext context)
        {
            this.context = context;
        }

        public async Task<Guid?> GetSessionAsync(string username, string password, string device)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var employee = context.Employees.FirstOrDefault(e => e.Username == username && e.Password == password && e.LockingReason == null);
                if (employee != null)
                {

                    var session = context.Sessions.FirstOrDefault(s => s.EmployeeId == employee.Id && s.DeviceId == device && s.TerminationTs != null);
                    if (session != null)
                    {
                        await context.SessionActivities.AddAsync(new Models.People.SessionActivity() { SessionId = session.Id, Details =  "Bla"});
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return session.Id;
                    }
                    else
                    {
                        session = new Models.People.Session() { Employee = employee, DeviceId = device };
                        await context.Sessions.AddAsync(session);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return session.Id;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            } 
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(DataContext context, Guid sessionid)
        {
            List<Claim> list = null;
            var person = await context.Sessions.FirstOrDefaultAsync(p => p.Id == sessionid && p.Employee.LockingReason == null);
            if (person != null)
            {
                list = new List<Claim>();
                context.Claims.Where(c => c.PersonId == person.Id && c.RevokedTs == null).ToList().ForEach(x => list.Add(new Claim(x.ClaimType, x.ClaimValue)));
                list.Add(new Claim("sss", sessionid.ToString()));
                //list.Add(new Claim("ooo", person.Employee.OfficeId.ToString()));
            }

            return list;
        }

        public static async Task<bool> TerminateSessionAsync(DataContext context, Guid sessionid)
        {
            var session = await context.Sessions.FirstOrDefaultAsync(s => s.Id == sessionid);
            if (session != null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
