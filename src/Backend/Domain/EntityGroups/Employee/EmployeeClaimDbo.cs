using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.EntityGroups.Employee;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.Domain.EntityGroups.Employee
{
    public class EmployeeClaimDbo : BaseEmployeeClaim, IEntityDbo
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
