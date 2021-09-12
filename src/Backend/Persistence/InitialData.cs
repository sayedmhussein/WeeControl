using System;
using System.Collections.Generic;
using System.Linq;
using WeeControl.Backend.Domain.EntityGroup.EmployeeSchema;
using WeeControl.Backend.Domain.EntityGroup.Territory;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.SharedKernel.EntityGroup.Employee;
using WeeControl.SharedKernel.EntityGroup.Employee.Enums;
using WeeControl.SharedKernel.EntityGroup.Territory;
using WeeControl.SharedKernel.EntityGroup.Territory.Enums;

namespace WeeControl.Backend.Persistence
{
    public class InitialData
    {
        private readonly IMySystemDbContext context;
        
        private ITerritoryLists territoryLists;
        private IEmployeeLists employeeLists;
        private List<string> tags;

        public InitialData(IMySystemDbContext context)
        {
            this.context = context;
        }

        public void Init(ITerritoryLists territoryLists, IEmployeeLists employeeLists)
        {
            this.territoryLists = territoryLists;
            this.employeeLists = employeeLists;
            
            tags = new List<string>();
            foreach (var e in Enum.GetValues(typeof(ClaimTagEnum)).Cast<ClaimTagEnum>())
            {
                tags.Add(employeeLists.GetClaimTag(e));
            }
            
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
                    CountryId = territoryLists.GetCountryName(CountryEnum.USA),
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
                    EmployeeTitle = employeeLists.GetPersonalTitle(PersonalTitleEnum.Mr),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Gender = employeeLists.GetPersonalGender(PersonalGenderEnum.Male),
                    TerritoryId = territoryid,
                    Username = "admin",
                    Password = "admin"
                },
                new EmployeeDbo()
                {
                    EmployeeTitle = employeeLists.GetPersonalTitle(PersonalTitleEnum.Mr),
                    FirstName = "User",
                    LastName = "User",
                    Gender = employeeLists.GetPersonalGender(PersonalGenderEnum.Male),
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
            List<EmployeeClaimDbo> list = new()
            {
                new EmployeeClaimDbo()
                {
                    EmployeeId = employeeid,
                    GrantedById = employeeid,
                    ClaimType = employeeLists.GetClaimType(ClaimTypeEnum.HumanResources),
                    ClaimValue = string.Join(";", tags)
                }
            };
            
            context.EmployeeClaims.AddRange(list);
            context.SaveChanges();
        }
    }
}