using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySystem.Data;

namespace MySystem.Api.Dtos.V1
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Device { get; set; }

        public async Task<Guid?> GetSessionAsync(DataContext context)
        {
            throw new NotImplementedException();
            //var employee = context;//context.Set<"">.FirstOrDefault(e => e.Username == Username && e.Password == Password);
            //if (employee != null)
            //{
                
            //    var session = context.Sessions.FirstOrDefault(s => s.PersonId == employee.Id && s.DeviceId == Device && s.TerminationTs != null);
            //    if (session != null)
            //    {
            //        return session.Id;
            //    }
            //    else
            //    {
            //        session = new Models.People.Session() { Person = employee, DeviceId = Device };
            //        await context.Sessions.AddAsync(session);
            //        await context.SaveChangesAsync();
            //        return session.Id;
            //    }
            //}
            //else
            //{
            //    return null;
            //}
        }

        public static async Task<IEnumerable<Claim>> GetUserClaimsAsync(DataContext context, Guid sessionid)
        {
            var list = new List<Claim>();
            var person = await context.Sessions.FirstOrDefaultAsync(p => p.Id == sessionid);
            if (person != null)
            {
                context.Claims.Where(c => c.PersonId == person.Id && c.RevokedTs == null).ToList().ForEach(x => list.Add(new Claim(x.ClaimType, x.ClaimValue)));
            }

            return list;
        }

        public static async Task<bool> TerminateSessionAsync(DataContext context, Guid sessionid)
        {
            var session = await context.Sessions.FirstOrDefaultAsync(s => s.Id == sessionid);
            context.Sessions.Remove(session);
            return true;
        }
    }
}
