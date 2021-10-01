using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.Server.Domain.Administration.Entities;
using WeeControl.Server.Domain.Common.Interfaces;
using WeeControl.Server.Domain.HumanResources.ValueObjects;
using WeeControl.SharedKernel.HumanResources.Bases;

namespace WeeControl.Server.Domain.HumanResources.Entities
{
    public class Employee : EmployeeBase, IAggregateRoot
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [NotMapped] 
        public string EmployeeName => FirstName + " " + LastName;
        
        public virtual Territory Territory { get; set; }
        public string TerritoryCode { get; set; }

        public string DepartmentCode { get; set; }

        public string PositionCode { get; set; }

        public string AccountSuspensionArgument { get; set; }

        public ICollection<Address> Addresses { get; set; }
        
        public ICollection<Identity> Identities { get; set; }
        
        public ICollection<Contact> Contacts { get; set; }

        

        private Employee()
        {
        }

        public static Employee Create(string territoryCode, string firstName, string lastName, string username, string password)
        {
            var employee = new Employee() 
            {
                TerritoryCode = territoryCode,
                FirstName = firstName, 
                LastName = lastName
            };

            return employee;
        }
    }
}