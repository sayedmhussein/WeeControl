using System;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.ApiApp.Persistence.DbContexts
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<PersonDbo> Person { get; set; }
        public DbSet<PersonIdentityDbo> PersonIdentities { get; set; }
        public DbSet<PersonContactDbo> PersonContacts { get; set; }
        public DbSet<AddressDbo> PersonAddresses { get; set; }
        public DbSet<UserDbo> Users { get; set; }
        public DbSet<UserClaimDbo> UserClaims { get; set; }
        public DbSet<EmployeeDbo> Employees { get; set; }
        public DbSet<CustomerDbo> Customers { get; set; }
        public DbSet<UserFeedsDbo> Feeds { get; set; }
        public DbSet<UserNotificationDbo> UserNotifications { get; set; }
        public DbSet<UserSessionDbo> UserSessions { get; set; }
        public DbSet<UserSessionLogDbo> SessionLogs { get; set; }

        public EssentialDbContext(DbContextOptions<EssentialDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IDbContext).Assembly);
            

            if (Database.IsMySql())
            {
                ConfigureMySql(modelBuilder);
            }
            else if (Database.IsSqlite())
            {
                ConfigureSqlite(modelBuilder);
            }
        }

        private static void ConfigureSqlite(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaimDbo>().Property(x => x.GrantedTs)
                .HasDefaultValue(DateTime.UtcNow);
            
            modelBuilder.Entity<UserFeedsDbo>().Property(x => x.FeedTs)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<UserNotificationDbo>().Property(x => x.PublishedTs)
                .HasDefaultValue(DateTime.UtcNow);
            
            modelBuilder.Entity<UserSessionDbo>().Property(x => x.CreatedTs)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<UserSessionLogDbo>().Property(x => x.LogTs)
                .HasDefaultValue(DateTime.UtcNow);
        }

        private static void ConfigureMySql(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonDbo>().ToTable(nameof(PersonDbo));
            modelBuilder.Entity<PersonIdentityDbo>().ToTable(nameof(PersonIdentityDbo));
            modelBuilder.Entity<AddressDbo>().ToTable(nameof(AddressDbo));
            modelBuilder.Entity<PersonContactDbo>().ToTable(nameof(PersonContactDbo));
            
            modelBuilder.Entity<UserDbo>().ToTable(nameof(UserDbo));
            modelBuilder.Entity<UserClaimDbo>().ToTable(nameof(UserClaimDbo));
            modelBuilder.Entity<UserClaimDbo>().Property(x => x.GrantedTs)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<EmployeeDbo>().ToTable(nameof(EmployeeDbo));
            modelBuilder.Entity<CustomerDbo>().ToTable(nameof(CustomerDbo));
            modelBuilder.Entity<UserFeedsDbo>().ToTable(nameof(UserFeedsDbo));
            modelBuilder.Entity<UserFeedsDbo>().Property(x => x.FeedTs)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<UserNotificationDbo>().ToTable(nameof(UserNotificationDbo));
            modelBuilder.Entity<UserNotificationDbo>().Property(x => x.PublishedTs)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<UserSessionDbo>().ToTable(nameof(UserSessionDbo));
            modelBuilder.Entity<UserSessionDbo>().Property(x => x.CreatedTs)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<UserSessionLogDbo>().ToTable(nameof(UserSessionLogDbo));
            modelBuilder.Entity<UserSessionLogDbo>().Property(x => x.LogTs)
                .ValueGeneratedOnAdd();
        }
    }
}
