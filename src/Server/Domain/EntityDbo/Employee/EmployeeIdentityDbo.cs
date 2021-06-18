using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.EntityDbo.EmployeeSchema;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using WeeControl.SharedKernel.CommonSchemas.Employee.Bases;

namespace WeeControl.Server.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeIdentityDbo : BaseEmployeeIdentity, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
