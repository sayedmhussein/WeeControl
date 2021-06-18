using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Bases;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
{
    public class EmployeeSessionLogDbo : BaseEmployeeSessionLog, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
