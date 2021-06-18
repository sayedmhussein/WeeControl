using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using WeeControl.SharedKernel.CommonSchemas.Employee.Bases;

namespace WeeControl.Server.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeSessionLogDbo : BaseEmployeeSessionLog, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
