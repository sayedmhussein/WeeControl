using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WeeControl.Backend.Domain.EntityGroup.EmployeeSchema;
using WeeControl.Backend.Domain.EntityGroup.Territory;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.SharedKernel.EntityGroup.Employee;
using WeeControl.SharedKernel.EntityGroup.Employee.Enums;
using WeeControl.SharedKernel.EntityGroup.Territory;
using WeeControl.SharedKernel.EntityGroup.Territory.Enums;

namespace WeeControl.Backend.Persistence
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }

    public class MySystemDbContext : DbContext, IMySystemDbContext
    {
        internal static DatabaseFacade DbFacade { get; private set; }

        public MySystemDbContext(DbContextOptions<MySystemDbContext> options) : base(options)
        {
            DbFacade = Database;

            //Database.EnsureDeleted(); //During Initial Development Only
            Database.EnsureCreated(); //During Initial Development Only

            if (Territories.Any() == false)
            {
                AddSuperUser();
                AddStandardUser();
            }
        }

        //Territory Schema
        public DbSet<TerritoryDbo> Territories { get; set; }

        //Employee Schema
        public DbSet<EmployeeDbo> Employees { get; set; }
        //
        public DbSet<EmployeeClaimDbo> EmployeeClaims { get; set; }
        public DbSet<EmployeeIdentityDbo> EmployeeIdentities { get; set; }
        //
        public DbSet<EmployeeSessionDbo> EmployeeSessions { get; set; }
        public DbSet<EmployeeSessionLogDbo> EmployeeSessionLogs { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp");

                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {        
                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(property.Name.ToSnakeCase());
                    }

                    foreach (var key in entity.GetKeys())
                    {
                        key.SetName(key.GetName().ToSnakeCase());
                    }

                    foreach (var key in entity.GetForeignKeys())
                    {
                        key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                    }

                    foreach (var index in entity.GetIndexes())
                    {
                        index.SetDatabaseName(index.Name.ToSnakeCase());
                    }
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MySystemDbContext).Assembly);
        }

        private void AddSuperUser()
        {
            ITerritoryLists territories = new TerritoryLists();
            IEmployeeLists employeeLists = new EmployeeLists();

            var territory = new TerritoryDbo()
            {
                CountryId = territories.GetCountryName(CountryEnum.USA),
                Name = "Head Office in USA"
            };
            Territories.Add(territory);
            SaveChanges();

            var superuser = new EmployeeDbo()
            {
                EmployeeTitle = employeeLists.GetPersonalTitle(PersonalTitleEnum.Mr),
                FirstName = "Admin",
                LastName = "Admin",
                Gender = employeeLists.GetPersonalGender(PersonalGenderEnum.Male),
                TerritoryId = territory.Id,
                Username = "admin",
                Password = "admin"
            };
            Employees.Add(superuser);
            SaveChanges();

            var tags = new List<string>();
            foreach (var e in Enum.GetValues(typeof(ClaimTagEnum)).Cast<ClaimTagEnum>())
            {
                tags.Add(employeeLists.GetClaimTag(e));
            }

            var superuserclaim = new EmployeeClaimDbo()
            {
                Employee = superuser,
                GrantedById = superuser.Id,
                ClaimType = employeeLists.GetClaimType(ClaimTypeEnum.HumanResources),
                ClaimValue = string.Join(";", tags)
            };
            EmployeeClaims.Add(superuserclaim);
            SaveChanges();
        }

        private void AddStandardUser()
        {
            IEmployeeLists employeeLists = new EmployeeLists();

            var standardUser = new EmployeeDbo()
            {
                EmployeeTitle = employeeLists.GetPersonalTitle(PersonalTitleEnum.Mr),
                FirstName = "User",
                LastName = "User",
                Gender = employeeLists.GetPersonalGender(PersonalGenderEnum.Male),
                TerritoryId = Territories.FirstOrDefault().Id,
                Username = "user",
                Password = "user"
            };
            Employees.Add(standardUser);
            SaveChanges();
        }
    }
}
