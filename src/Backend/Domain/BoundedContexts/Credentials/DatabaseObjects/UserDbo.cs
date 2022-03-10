using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Backend.Domain.Credentials.DatabaseObjects
{
    public class UserDbo : User
    {
        [Key]
        public Guid UserId { get; set; }

        public IEnumerable<SessionDbo> Sessions { get; set; }
    }
}
