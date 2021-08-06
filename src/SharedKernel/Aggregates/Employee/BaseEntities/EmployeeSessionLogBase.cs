using System;
namespace WeeControl.SharedKernel.Aggregates.Employee.BaseEntities
{
    public abstract class BaseEmployeeSessionLog
    {
        public Guid SessionId { get; set; }

        public DateTime LogTs { get; set; } = DateTime.Now;

        public string LogDetails { get; set; }
    }
}
