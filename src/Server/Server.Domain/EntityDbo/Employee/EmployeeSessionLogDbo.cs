using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBases.Employee;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeSessionLogDbo : EmployeeSessionBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
