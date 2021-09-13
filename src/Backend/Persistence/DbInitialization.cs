using System;
using System.Collections.Generic;
using System.Linq;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Domain.EntityGroups.Employee;
using WeeControl.Backend.Domain.EntityGroups.Territory;
using WeeControl.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.SharedKernel.EntityGroups.Territory.Interfaces;

namespace WeeControl.Backend.Persistence
{
    public class DbInitialization
    {
        private readonly IMySystemDbContext context;
        
        private ITerritoryAttribute territoryAttribute;
        private IEmployeeAttribute employeeAttribute;

        public DbInitialization(IMySystemDbContext context)
        {
            this.context = context;
        }

        public void Init(ITerritoryAttribute territoryAttribute, IEmployeeAttribute employeeAttribute)
        {
            this.territoryAttribute = territoryAttribute;
            this.employeeAttribute = employeeAttribute;
            
            
            
            AddTerritores();
            AddEmployees();
            AddEmployeeClaims();
        }

        private void AddTerritores()
        {
            List<TerritoryDbo> list = new()
            {
                new TerritoryDbo()
                {
                    CountryId = territoryAttribute.GetCountryName(CountryEnum.USA),
                    Name = "Head Office in USA"
                }
            };
            
            context.Territories.AddRange(list);
            context.SaveChanges();
        }

        private void AddEmployees ()
        {
            var territoryid = context.Territories.First().Id;
            
            List<EmployeeDbo> list = new()
            {
                new EmployeeDbo()
                {
                    EmployeeTitle = employeeAttribute.GetPersonalTitle(PersonalTitleEnum.Mr),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Gender = employeeAttribute.GetPersonalGender(PersonalGenderEnum.Male),
                    TerritoryId = territoryid,
                    Username = "admin",
                    Password = "admin"
                },
                new EmployeeDbo()
                {
                    EmployeeTitle = employeeAttribute.GetPersonalTitle(PersonalTitleEnum.Mr),
                    FirstName = "User",
                    LastName = "User",
                    Gender = employeeAttribute.GetPersonalGender(PersonalGenderEnum.Male),
                    TerritoryId = territoryid,
                    Username = "user",
                    Password = "user"
                }
            };
            
            context.Employees.AddRange(list);
            context.SaveChanges();
        }

        private void AddEmployeeClaims()
        {
            var employeeid = context.Employees.First(x => x.Username == "admin").Id;
            var tags = Enum.GetValues(typeof(ClaimTagEnum))
                .Cast<ClaimTagEnum>()
                .Select(e => employeeAttribute.GetClaimTag(e))
                .ToList();

            List<EmployeeClaimDbo> list = new()
            {
                new EmployeeClaimDbo()
                {
                    EmployeeId = employeeid,
                    GrantedById = employeeid,
                    ClaimType = employeeAttribute.GetClaimType(ClaimTypeEnum.HumanResources),
                    ClaimValue = string.Join(";", tags)
                }
            };
            
            context.EmployeeClaims.AddRange(list);
            context.SaveChanges();
        }
    }
}