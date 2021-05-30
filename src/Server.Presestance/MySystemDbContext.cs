using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySystem.Application.Common.Interfaces;
using MySystem.Persistence.Infrastructure.EfRepository.Models.Business;
using MySystem.Persistence.EntityTypeConfiguration;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Persistence.EntityTypeConfiguration.EmployeeSchema;
using MySystem.Persistence.EntityTypeConfiguration.PublicSchema;
using MySystem.Domain.EntityDbo.PublicSchema;
using MySystem.Domain.EntityDbo.UnitSchema;
using MySystem.Domain.EntityDbo.ContractSchema;

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

            SeedBasicDbs();
            SeedPeople();
            SeedComponentDbs();
            SeedBusinessDbs(); 
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

        private void SeedBasicDbs()
        {
            if (Territories.Any() == false)
            {
                Territories.AddRange(TerritoryDbo.InitializeList());
                SaveChanges();
            }

            if (Buildings.Any() == false)
            {
                Buildings.AddRange(BuildingDbo.InitializeList());
                SaveChanges();
            }
        }

        private void SeedBusinessDbs()
        {
            if (Contracts.Any() == false)
            {
                var office = Territories.FirstOrDefault();
                var sales = Employees.FirstOrDefault();
                Contracts.AddRange(Contract.GetContractList(office.Id, sales.Id));
                SaveChanges();
            }

            //if (ContractUnits.Any() == false)
            //{
            //    var contract = Contracts.First().Id;
            //    var unit = Units.First().Id;
            //    ContractUnits.AddRange(ContractUnit.GetContractUnitList(contract, unit));
            //    SaveChanges();
            //}
        }

        private void SeedComponentDbs()
        {
            //if (Units.Any() == false)
            //{
            //    //Units.AddRange(Unit.GetUnitList(Buildings.First().Id));
            //    SaveChanges();
            //}
        }

        private void SeedPeople()
        {
            if (Employees.Any() == false)
            {
                var office = Territories.FirstOrDefault();
                Employees.AddRange(EmployeeDbo.InitializeList(office.Id));
                SaveChanges();

                var employee = Employees.FirstOrDefault(x => x.Username == "username");
                EmployeeClaims.AddRange(EmployeeClaimDbo.InitializeList(employee.Id));

                SaveChanges();
            }
        }
    }
}
