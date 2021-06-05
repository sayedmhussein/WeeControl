using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Entities.Employee.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeIdentityDbo : EmployeeIdentityBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
