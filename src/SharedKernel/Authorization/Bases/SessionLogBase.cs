using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Authorization.Bases
{
    public abstract class SessionLogBase : IDataTransferObject
    {
        [Required]
        public DateTime LogTs { get; set; }

        [StringLength(10)]
        public string LogType { get; set; }

        [StringLength(255)]
        public string LogValue { get; set; }
    }
}