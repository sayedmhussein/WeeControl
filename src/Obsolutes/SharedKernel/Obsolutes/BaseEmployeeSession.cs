using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Obsolutes
{
    public abstract class BaseEmployeeSession : IVerifyable
    {
        public Guid EmployeeId { get; set; }

        [StringLength(128, ErrorMessage = "Device ID length must not exceed 128 letter.")]
        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; } = DateTime.UtcNow;

        public DateTime? TerminationTs { get; set; }
    }
}
