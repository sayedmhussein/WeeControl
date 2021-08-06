using System;

namespace WeeControl.SharedKernel.Aggregates.Employee.BaseEntities
{
    public abstract class BaseEmployeeSession
    {
        public Guid EmployeeId { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; }

        public DateTime? TerminationTs { get; set; }
    }
}
