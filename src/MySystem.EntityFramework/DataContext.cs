using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.EntityFramework.Models;
using Sayed.MySystem.EntityFramework.Models.Basic;
using Sayed.MySystem.EntityFramework.Models.Business;
using Sayed.MySystem.EntityFramework.Models.Component;
using Sayed.MySystem.EntityFramework.Models.People;
using Sayed.MySystem.Shared.Dbos;

namespace Sayed.MySystem.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureDeleted(); //During Initial Development Only
            Database.EnsureCreated(); //During Initial Development Only

            SeedBasicDbs();
            SeedPeople();
            SeedComponentDbs();
            SeedBusinessDbs();
        }

        //Basic Schema
        internal DbSet<OfficeDbo> Offices { get; set; }
        internal DbSet<BuildingDbo> Buildings { get; set; }

        //Employee Schema
        internal DbSet<EmployeeDbo> Employees { get; set; }       
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionActivity> SessionActivities { get; set; }
        public DbSet<Claim> Claims { get; set; }

        //Component Schema
        internal DbSet<Unit> Units { get; set; }

        //Business Schema
        internal DbSet<Contract> Contracts { get; set; }
        internal DbSet<ContractUnit> ContractUnits { get; set; }
        internal DbSet<Visit> Visits { get; set; }
        internal DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Office.CreateModelBuilder(this, modelBuilder);
            Building.CreateModelBuilder(this, modelBuilder);

            Employee.CreateModelBuilder(this, modelBuilder);
            Session.CreateSessionModel(this, modelBuilder);
            SessionActivity.CreateSessionActivityModel(this, modelBuilder);
            Claim.CreateClaimModel(this, modelBuilder);

            Unit.CreateUnitModel(this, modelBuilder);

            Contract.CreateUnitModel(this, modelBuilder);
            ContractUnit.CreateContractUnitModel(this, modelBuilder);
        }

        private void SeedBasicDbs()
        {
            if (Offices.Any() == false)
            {
                Offices.AddRange(Office.GetOfficeList());
                SaveChanges();
            }

            if (Buildings.Any() == false)
            {
                Buildings.AddRange(Building.GetOfficeList());
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
                var contract = Contracts.First().ContractId;
                var unit = Units.First().UnitId;
                ContractUnits.AddRange(ContractUnit.GetContractUnitList(contract, unit));
                SaveChanges();
            }
        }

        private void SeedComponentDbs()
        {
            if (Units.Any() == false)
            {
                Units.AddRange(Unit.GetUnitList(Buildings.First().Id));
                SaveChanges();
            }
        }

        private void SeedPeople()
        {
            if (Employees.Any() == false)
            {
                var office = Offices.FirstOrDefault(x => x.OfficeName == "Mecca");
                Employees.AddRange(Employee.GetPersonList(office.Id));
                SaveChanges();
            }
        }
    }
}
