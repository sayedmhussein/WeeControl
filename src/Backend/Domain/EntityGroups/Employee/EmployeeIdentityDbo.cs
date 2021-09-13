using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.EntityGroups.Employee;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.Domain.EntityGroups.Employee
{
    public class EmployeeIdentityDbo : BaseEmployeeIdentity, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
