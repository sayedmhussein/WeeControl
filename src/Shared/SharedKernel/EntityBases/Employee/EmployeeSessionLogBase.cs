using System;
namespace MySystem.SharedKernel.EntityBases.Employee
{
    public abstract class EmployeeSessionLogBase
    {
        public Guid SessionId { get; set; }

        public DateTime LogTs { get; set; } = DateTime.Now;

        public string LogDetails { get; set; }
    }
}
