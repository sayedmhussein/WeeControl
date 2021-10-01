using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Authorization.Bases
{
    public abstract class SessionBase : IDataTransferObject
    {
        [Required]
        public DateTime CreationTs { get; set; } = DateTime.UtcNow;
        
        public DateTime? TerminationTs { get; set; }
        
        [Required]
        [StringLength(128, ErrorMessage = "Device ID length must not exceed 128 letter.")]
        public string DeviceId { get; set; }
    }
}