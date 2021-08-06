using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Aggregates.Employee.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
{
    public class EmployeeSessionLogDbo : BaseEmployeeSessionLog, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
