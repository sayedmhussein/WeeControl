using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Backend.Domain.Essential.Entities;

public class SessionDbo
{
    public static SessionDbo Create(Guid userid, string deviceid)
    {
        return new SessionDbo() { UserId = userid, DeviceId = deviceid, CreatedTs = DateTime.UtcNow };
    }
    
    [Key]
    public Guid SessionId { get; set; }

    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    [Required]
    public string DeviceId { get; set; }

    public DateTime CreatedTs { get; set; }

    public DateTime? TerminationTs { get; set; }

    public SessionLogDbo CreateLog(string context, string details)
    {
        return new SessionLogDbo() { SessionId = this.SessionId, LogTs = DateTime.UtcNow, Context = context, Details = details };
    }

    private SessionDbo()
    {
    }
}