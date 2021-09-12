using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.EntityGroup.Employee.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.Domain.EntityGroup.EmployeeSchema
{
    public class EmployeeIdentityDbo : BaseEmployeeIdentity, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
