using System;
using System.Collections.Generic;

namespace WeeControl.Domain.Contexts.Essential;

public class SessionDbo
{
    public static SessionDbo Create(Guid userid, string deviceid)
    {
        return new SessionDbo() { UserId = userid, DeviceId = deviceid, CreatedTs = DateTime.UtcNow };
    }
    
    public Guid SessionId { get; }
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }
    public string DeviceId { get; set; }
    public DateTime CreatedTs { get; set; }
    public DateTime? TerminationTs { get; set; }

    public virtual IEnumerable<SessionLogDbo> Logs { get; set; }

    public SessionLogDbo CreateLog(string context, string details)
    {
        return new SessionLogDbo() { SessionId = this.SessionId, LogTs = DateTime.UtcNow, Context = context, Details = details };
    }

    private SessionDbo()
    {
    }
}