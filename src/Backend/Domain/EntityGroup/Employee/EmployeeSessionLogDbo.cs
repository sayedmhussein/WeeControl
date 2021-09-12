using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.EntityGroup.Employee.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.Domain.EntityGroup.EmployeeSchema
{
    public class EmployeeSessionLogDbo : BaseEmployeeSessionLog, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
