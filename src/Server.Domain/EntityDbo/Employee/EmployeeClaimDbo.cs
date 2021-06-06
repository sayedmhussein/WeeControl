using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBases.Employee;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeClaimDbo : EmployeeClaimBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual EmployeeDbo Employee { get; set; }

        //[ForeignKey(nameof(GrantedById))]
        //public virtual EmployeeDbo GrantedBy { get; set; }

        //[ForeignKey(nameof(RevokedById))]
        //public virtual EmployeeDbo RevokedBy { get; set; }˚
    }
}
