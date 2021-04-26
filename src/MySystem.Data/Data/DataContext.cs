using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models;
using MySystem.Data.Models.Basic;
using MySystem.Data.Models.Business;
using MySystem.Data.Models.Component;
using MySystem.Data.Models.People;

namespace MySystem.Data.Data
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
        internal DbSet<Office> Offices { get; set; }
        internal DbSet<Building> Buildings { get; set; }

        //People Schema
        internal DbSet<Person> People { get; set; }
        internal DbSet<Employee> Employees { get; set; }
        public DbSet<Session> Sessions { get; set; }
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
            Office.CreateOfficeModel(this, modelBuilder);
            Building.CreateBuildingModel(this, modelBuilder);

            Person.CreatePersonModel(this, modelBuilder);
            Employee.CreateEmployeeModel(this, modelBuilder);
            Session.CreateSessionModel(this, modelBuilder);
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
                People.AddRange(Employee.GetPersonList(office.Id));
                SaveChanges();
            }
        }
    }
}
