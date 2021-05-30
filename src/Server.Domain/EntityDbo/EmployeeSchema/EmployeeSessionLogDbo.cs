using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeSessionLogDbo : SessionLogBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
