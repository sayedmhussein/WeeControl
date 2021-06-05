using System;

namespace MySystem.SharedKernel.Entities.Employee.Base
{
    public class EmployeeSessionBase
    {
        public Guid EmployeeId { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; }

        public DateTime? TerminationTs { get; set; }
    }
}
