using System;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;
using WeeControl.Backend.Persistence.Credentials.Configurations;

namespace WeeControl.Backend.Persistence.Credentials
{
    public class CredentialsDbContext : DbContext, ICredentialsDbContext
    {
        public DbSet<UserDbo> Users { get; set; }

        public DbSet<SessionDbo> Sessions { get; set; }

        public CredentialsDbContext(DbContextOptions<CredentialsDbContext> options) : base(options)
        {
            Database.EnsureCreated();

            Users.Add(new UserDbo() { Username = "admin", Password = "admin" });
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
        }
    }
}
