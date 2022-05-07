using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects.EssentialsObjects
{
    public class SessionDbo
    {
        [Key]
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }
        public UserDbo User { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreatedTs { get; set; }

        public DateTime? TerminationTs { get; set; }

        public static SessionDbo Create(Guid userid, string deviceid)
        {
            return new SessionDbo() { UserId = userid, DeviceId = deviceid, CreatedTs = DateTime.UtcNow };
        }
    }
}
