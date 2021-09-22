using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.Common.SharedKernel.Abstract.Entities;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities
{
    public class Employee : PersonBase, IAggregateRoot
    {
        public Guid EmployeeId { get; set; }

        [NotMapped] 
        public string EmployeeName => FirstName + LastName;

        public Credentials Credentials { get; set; }

        public ICollection<Address> Addresses { get; set; }
        
        public ICollection<Identity> Identities { get; set; }
        
        public ICollection<Contact> Contacts { get; set; }

        private Employee()
        {
        }
        

        public static Employee Create(IPerson person, IEnumerable<Address> addresses, IEnumerable<Identity> identities, IEnumerable<Contact> contacts)
        {
            throw new NotImplementedException();
        }
        
        
    }
}