using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Backend.Domain.Credentials.DatabaseObjects
{
    public class SessionDbo : Session
    {
        [Key]
        public Guid SessionId { get; set; }

        public User User { get; set; }
    }
}
