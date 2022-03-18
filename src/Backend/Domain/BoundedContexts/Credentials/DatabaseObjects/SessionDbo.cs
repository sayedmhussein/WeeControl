using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Backend.Domain.Credentials.DatabaseObjects
{
    public class SessionDbo : Session
    {
        [Key]
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }
        public UserDbo User { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreatedTs { get; set; }

        public DateTime? TerminationTs { get; set; }
    }
}
