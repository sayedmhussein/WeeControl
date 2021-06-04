using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySystem.Application.Common.Interfaces;
using MySystem.Persistence.Infrastructure.EfRepository.Models.Business;
using MySystem.Persistence.EntityTypeConfiguration;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Persistence.EntityTypeConfiguration.Employee;
using MySystem.Persistence.EntityTypeConfiguration.PublicSchema;
using MySystem.Domain.EntityDbo.PublicSchema;
using MySystem.Domain.EntityDbo.UnitSchema;
using MySystem.Domain.EntityDbo.ContractSchema;
using MySystem.SharedKernel.Entities.Public.Constants;

namespace MySystem.Persistence
{
    public class MySystemDbContext : DbContext, IMySystemDbContext
    {
        internal static DatabaseFacade DbFacade { get; private set; }

        public MySystemDbContext(DbContextOptions<MySystemDbContext> options) : base(options)
        {
            DbFacade = Database;

            //Database.EnsureDeleted(); //During Initial Development Only
            Database.EnsureCreated(); //During Initial Development Only

            AddSuperUser();
        }

        //Basic Schema
        public DbSet<TerritoryDbo> Territories { get; set; }
        

        //Employee Schema
        public DbSet<EmployeeDbo> Employees { get; set; }
        //
        public DbSet<EmployeeClaimDbo> EmployeeClaims { get; set; }
        public DbSet<EmployeeIdentityDbo> EmployeeIdentities { get; set; }
        //
        public DbSet<EmployeeSessionDbo> EmployeeSessions { get; set; }
        public DbSet<EmployeeSessionLogDbo> EmployeeSessionLogs { get; set; }
        

        //Component Schema
        public DbSet<UnitDbo> Units { get; set; }

        //Business Schema
        public DbSet<BuildingDbo> Buildings { get; set; }
        public DbSet<ContractDbo> Contracts { get; set; }
        public DbSet<ContractUnitDbo> ContractUnits { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp");
            }

            //Public Schema
            new TerritoryEntityTypeConfiguration().Configure(modelBuilder.Entity<TerritoryDbo>());

            //Employee Schema
            new EmployeeEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeDbo>());
            //
            new EmployeeClaimEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeClaimDbo>());
            new EmployeeIdentityEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeIdentityDbo>());
            //
            new EmployeeSessionClaimEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeSessionDbo>());
            new EmployeeSessionLogEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeSessionLogDbo>());




            new BuildingEntityTypeConfiguration().Configure(modelBuilder.Entity<BuildingDbo>());
            //Unit.CreateModelBuilder(this, modelBuilder);

            Contract.CreateModelBuilder(this, modelBuilder);
            ContractUnit.CreateContractUnitModel(this, modelBuilder);
        }

        private void AddSuperUser()
        {
            var territory = new TerritoryDbo()
            {
                CountryId = Counties.List[Counties.Name.USA],
                OfficeName = "Head Office in USA"
            };
            Territories.Add(territory);
            SaveChanges();

            var superuser = new EmployeeDbo()
            {
                EmployeeTitle = Titles.List[Titles.Title.Mr],
                FirstName = "Admin",
                LastName = "Admin",
                Gender = Genders.List[Genders.Gender.Male],
                TerritoryId = territory.Id,
                Username = "admin",
                Password = "admin"
            };
            Employees.Add(superuser);
            SaveChanges();
        }
    }
}
