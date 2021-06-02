using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Entities.Employee.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeSessionDbo : EmployeeSessionBase, IEntityDbo
    {
        public static IEnumerable<EmployeeSessionDbo> InitializeList(Guid employeeid)
        {
            return new List<EmployeeSessionDbo>();
        }

        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Employee { get; set; }

        public virtual ICollection<EmployeeSessionLogDbo> Logs { get; set; }
    }
}
