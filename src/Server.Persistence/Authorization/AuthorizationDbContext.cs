using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.Authorization.Entities;
using WeeControl.Server.Persistence.Authorization.Configurations;

namespace WeeControl.Server.Persistence.Authorization
{
    public class AuthorizationDbContext : DbContext, IAuthorizationDbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<UserClaim> Claims { get; set; }
        public DbSet<UserSession> Sessions { get; set; }

        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserSessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimEntityTypeConfiguration());
        }
    }
}