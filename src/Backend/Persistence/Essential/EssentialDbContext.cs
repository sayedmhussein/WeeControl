﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.EssentialContext;
using WeeControl.Backend.Domain.Essential.Entities;
using WeeControl.Backend.Persistence.Essential.Configurations;
using WeeControl.Common.SharedKernel.Essential;
using WeeControl.Common.SharedKernel.Services;

namespace WeeControl.Backend.Persistence.Essential
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<UserDbo> Users { get; set; }

        public DbSet<SessionDbo> Sessions { get; set; }
        
        public DbSet<SessionLogDbo> Logs { get; set; }

        public DbSet<TerritoryDbo> Territories { get; set; }

        public DbSet<ClaimDbo> Claims { get; set; }

        public EssentialDbContext(DbContextOptions<EssentialDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
            Database.EnsureCreated();

            if (!Territories.Any())
            {
                var territory = new TerritoryDbo() { CountryCode = "EGP", TerritoryCode = "CAI", TerritoryName = "Cairo" };
                Territories.Add(territory);
                SaveChanges();

                var user = UserDbo.Create("admin@admin.com", "admin", new PasswordSecurity().Hash("admin"), territory.TerritoryCode);
                Users.Add(user);
                SaveChanges();

                var claim = new ClaimDbo() { UserId = user.UserId, ClaimType = HumanResourcesData.Role, ClaimValue = HumanResourcesData.Claims.Tags.SuperUser };
                Claims.Add(claim);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TerritoryEntityTypeConfiguration());
        }
    }
}
