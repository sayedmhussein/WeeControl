using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Aggregates.Employee.Entities;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
{
    public class EmployeeSessionLogDbo : BaseEmployeeSessionLog, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
