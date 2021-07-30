using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Aggregates.Employee.Entities;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
{
    public class EmployeeIdentityDbo : BaseEmployeeIdentity, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
