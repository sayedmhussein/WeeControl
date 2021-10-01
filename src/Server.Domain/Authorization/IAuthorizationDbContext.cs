using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Domain.Authorization.Entities;
using WeeControl.Server.Domain.Common.Interfaces;

namespace WeeControl.Server.Domain.Authorization
{
    public interface IAuthorizationDbContext : IDbContext
    {
        DbSet<User> Users { get; set; }

        DbSet<UserClaim> Claims { get; set; }
        
        DbSet<UserSession> Sessions { get; set; }
    }
}