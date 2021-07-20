using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.BasicDbos.EmployeeSchema;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Entities;

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
