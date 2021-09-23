using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities
{
    public class Session : IAggregateRoot
    {
        [Key]
        public Guid SessionId { get; set; }

        public virtual Employee Employee { get; set; }
        public Guid EmployeeId { get; set; }
        
        [StringLength(128, ErrorMessage = "Device ID length must not exceed 128 letter.")]
        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; } = DateTime.UtcNow;

        public DateTime? TerminationTs { get; set; }

        public ICollection<SessionLog> Logs { get; set; }

        private Session()
        {
        }

        public static Session Create(Employee employee, string device)
        {
            throw new NotImplementedException();
        }
    }
}