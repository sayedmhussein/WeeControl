using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.Domain.EntityGroups.Employee
{
    public class EmployeeSessionLogDbo : BaseEmployeeSessionLog, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
