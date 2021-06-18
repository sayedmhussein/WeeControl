using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Domain.BasicDbos.EmployeeSchema;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory.Interfaces;

namespace WeeControl.Server.Persistence
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
            ITerritoryDicts territoryValues = new TerritoryDicts();
            IClaimDicts values = new ClaimDicts();
            IPersonalAttribDicts personalAttrib = new PersonalAttribDicts();

            var territory = new TerritoryDbo()
            {
                CountryId = territoryValues.Country[CountryEnum.USA],
                Name = "Head Office in USA"
            };
            Territories.Add(territory);
            SaveChanges();

            var superuser = new EmployeeDbo()
            {
                EmployeeTitle = personalAttrib.PersonTitle[PersonalTitleEnum.Mr],
                FirstName = "Admin",
                LastName = "Admin",
                Gender = personalAttrib.PersonGender[PersonalGenderEnum.Male],
                TerritoryId = territory.Id,
                Username = "admin",
                Password = "admin"
            };
            Employees.Add(superuser);
            SaveChanges();

            var superuserclaim = new EmployeeClaimDbo()
            {
                Employee = superuser,
                GrantedById = superuser.Id,
                ClaimType = values.ClaimType[ClaimTypeEnum.HumanResources],
                ClaimValue = string.Join(";", values.ClaimTag.Values)
            };
            EmployeeClaims.Add(superuserclaim);
            SaveChanges();
        }

        private void AddStandardUser()
        {
            IPersonalAttribDicts values = new PersonalAttribDicts();

            var standardUser = new EmployeeDbo()
            {
                EmployeeTitle = values.PersonTitle[PersonalTitleEnum.Mr],
                FirstName = "User",
                LastName = "User",
                Gender = values.PersonGender[PersonalGenderEnum.Male],
                TerritoryId = Territories.FirstOrDefault().Id,
                Username = "user",
                Password = "user"
            };
            Employees.Add(standardUser);
            SaveChanges();
        }
    }
}
