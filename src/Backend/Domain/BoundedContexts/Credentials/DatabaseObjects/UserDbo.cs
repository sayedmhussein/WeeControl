using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Backend.Domain.BoundedContexts.Credentials.DatabaseObjects;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Backend.Domain.Credentials.DatabaseObjects
{
    public class UserDbo : User
    {
        [Key]
        public Guid UserId { get; set; }

        public string TerritoryCode { get; set; }

        public DateTime? SuspendTs { get; set; }

        public virtual IEnumerable<SessionDbo> Sessions { get; set; }
        public virtual IEnumerable<ClaimDbo> Claims { get; set; }
    }
}
