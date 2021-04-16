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
    public partial class DataContext : DbContext
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
        public DbSet<Office> Offices { get; set; }
        public DbSet<Building> Buildings { get; set; }

        //People Schema
        public DbSet<Person> People { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //Component Schema
        public DbSet<Unit> Units { get; set; }

        //Business Schema
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractUnit> ContractUnits { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Office.CreateOfficeModel(this, modelBuilder);
            Building.CreateBuildingModel(this, modelBuilder);

            Person.CreatePersonModel(this, modelBuilder);
            Employee.CreateEmployeeModel(this, modelBuilder);

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
                Contracts.AddRange(Contract.GetContractList(office.OfficeId, sales.PersonId));
                SaveChanges();
            }
        }

        private void SeedComponentDbs()
        {
            if (Units.Any() == false)
            {
                Units.AddRange(Unit.GetUnitList(Buildings.First().BuildingId));
                SaveChanges();
            }
        }

        private void SeedPeople()
        {
            if (Employees.Any() == false)
            {
                var office = Offices.FirstOrDefault(x => x.OfficeName == "Mecca");
                People.AddRange(Employee.GetPersonList(office.OfficeId));
                SaveChanges();
            }
        }
    }
}
