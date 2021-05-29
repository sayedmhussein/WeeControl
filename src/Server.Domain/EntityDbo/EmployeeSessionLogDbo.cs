using System;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.Domain.EntityDbo
{
    public class EmployeeSessionLogDbo : SessionLogBase, IEntityDbo
    {
        public Guid Id { get; set; }

        public virtual EmployeeSessionDbo Session { get; set; }
    }
}
