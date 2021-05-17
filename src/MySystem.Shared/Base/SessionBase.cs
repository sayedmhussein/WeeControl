using System;
namespace Sayed.MySystem.Shared.Base
{
    public abstract class SessionBase
    {
        public Guid EmployeeId { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; }

        public DateTime? TerminationTs { get; set; }
    }
}
