﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.Credentials.DatabaseObjects;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;
using WeeControl.Backend.Persistence.Credentials.Configurations;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;

namespace WeeControl.Backend.Persistence.Credentials
{
    public class CredentialsDbContext : DbContext, ICredentialsDbContext
    {
        public DbSet<UserDbo> Users { get; set; }

        public DbSet<SessionDbo> Sessions { get; set; }

        public DbSet<TerritoryDbo> Territories { get; set; }

        public DbSet<ClaimDbo> Claims { get; set; }

        public CredentialsDbContext(DbContextOptions<CredentialsDbContext> options) : base(options)
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

                var user = new UserDbo() { Username = "admin", Password = "admin", TerritoryCode = territory.TerritoryCode };
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
