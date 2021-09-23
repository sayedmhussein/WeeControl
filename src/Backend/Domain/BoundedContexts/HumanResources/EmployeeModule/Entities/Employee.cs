using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.Common.SharedKernel.Abstract.Entities;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities
{
    public class Employee : PersonBase, IAggregateRoot
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [NotMapped] 
        public string EmployeeName => FirstName + " " + LastName;
        
        public virtual Territory Territory { get; set; }
        public string TerritoryCode { get; set; }
        
        public Credentials Credentials { get; set; }

        public ICollection<Address> Addresses { get; set; }
        
        public ICollection<Identity> Identities { get; set; }
        
        public ICollection<Contact> Contacts { get; set; }

        public ICollection<Claims> Claims { get; set; }

        private Employee()
        {
        }

        public static Employee Create(string territoryCode, string firstName, string lastName, string username, string password)
        {
            var employee = new Employee() 
            {
                TerritoryCode = territoryCode,
                FirstName = firstName, 
                LastName = lastName, 
                Credentials = new Credentials("admin", "admin")
            };

            return employee;
        }
    }
}