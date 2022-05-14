using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;

public class SessionLogDbo
{
    [Key]
    public Guid LogId { get; set; }
    
    public Guid SessionId { get; set; }
    public SessionDbo Session { get; set; }
    
    public DateTime LogTs { get; set; }

    public string Context { get; set; }

    public string Details { get; set; }

    internal SessionLogDbo()
    {
    }
}