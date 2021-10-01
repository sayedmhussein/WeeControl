using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.Authorization.ValueObjects;
using WeeControl.Server.Domain.Common.Interfaces;
using WeeControl.SharedKernel.Authorization.Bases;

namespace WeeControl.Server.Domain.Authorization.Entities
{
    public class UserSession : SessionBase, IAggregateRoot
    {
        [Key]
        public Guid SessionId { get; set; }

        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        
        public ICollection<SessionLog> Logs { get; set; }

        public UserSession()
        {
        }

        public UserSession(Guid userId, string device) : base()
        {
            
        }
    }
}