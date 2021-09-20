using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

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
