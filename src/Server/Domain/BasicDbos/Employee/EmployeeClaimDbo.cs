using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Bases;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
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
