using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySystem.Shared.Library.Dbo.Entity;
using MySystem.Web.Infrastructure.EfRepository.EntityTypeConfiguration;
using MySystem.Web.Infrastructure.EfRepository.Models.Business;
using MySystem.Web.Infrastructure.EfRepository.Models.People;

namespace MySystem.Web.Infrastructure.EfRepository
{
    public class DataContext : DbContext
    {
        internal static DatabaseFacade DbFacade { get; private set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
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
        public DbSet<OfficeDbo> Offices { get; set; }
        public DbSet<BuildingDbo> Buildings { get; set; }

        //Employee Schema
        public DbSet<EmployeeDbo> Employees { get; set; }       
        public DbSet<EmployeeSessionDbo> EmployeeSessions { get; set; }
        public DbSet<EmployeeClaimDbo> EmployeeClaims { get; set; }
        //
        public DbSet<SessionActivity> SessionActivities { get; set; }
        

        //Component Schema
        internal DbSet<UnitDbo> Units { get; set; }

        //Business Schema
        internal DbSet<ContractDbo> Contracts { get; set; }
        internal DbSet<ContractUnitDbo> ContractUnits { get; set; }
        internal DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp");
            }

            new BuildingEntityTypeConfiguration().Configure(modelBuilder.Entity<BuildingDbo>());
            new OfficeEntityTypeConfiguration().Configure(modelBuilder.Entity<OfficeDbo>());

            new EmployeeEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeDbo>());
            new EmployeeClaimEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeClaimDbo>());
            new EmployeeSessionClaimEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeSessionDbo>());

            //Employee.CreateModelBuilder(this, modelBuilder);
            SessionActivity.CreateModelBuilder(this, modelBuilder);

            //Unit.CreateModelBuilder(this, modelBuilder);

            Contract.CreateModelBuilder(this, modelBuilder);
            ContractUnit.CreateContractUnitModel(this, modelBuilder);
        }

        private void SeedBasicDbs()
        {
            if (Offices.Any() == false)
            {
                Offices.AddRange(OfficeDbo.InitializeList());
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
                var office = Offices.FirstOrDefault(x => x.OfficeName == "Mecca");
                var sales = Employees.FirstOrDefault();
                Contracts.AddRange(Contract.GetContractList(office.Id, sales.Id));
                SaveChanges();
            }

            if (ContractUnits.Any() == false)
            {
                var contract = Contracts.First().Id;
                var unit = Units.First().Id;
                ContractUnits.AddRange(ContractUnit.GetContractUnitList(contract, unit));
                SaveChanges();
            }
        }

        private void SeedComponentDbs()
        {
            if (Units.Any() == false)
            {
                //Units.AddRange(Unit.GetUnitList(Buildings.First().Id));
                SaveChanges();
            }
        }

        private void SeedPeople()
        {
            if (Employees.Any() == false)
            {
                var office = Offices.FirstOrDefault(x => x.OfficeName == "Mecca");
                Employees.AddRange(EmployeeDbo.InitializeList(office.Id));
                SaveChanges();

                var employee = Employees.FirstOrDefault(x => x.Username == "username");
                EmployeeClaims.AddRange(EmployeeClaimDbo.InitializeList(employee.Id));

                SaveChanges();
            }
        }
    }
}
