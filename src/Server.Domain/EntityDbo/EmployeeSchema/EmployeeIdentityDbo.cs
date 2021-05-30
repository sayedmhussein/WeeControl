using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeIdentityDbo : IdentityBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
