using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Backend.Domain.BoundedContexts.Credentials.DatabaseObjects
{
    public class UserDbo
    {
        [Key]
        public Guid UserId { get; set; }
        
        public string Email { get; set; }

        [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string TerritoryCode { get; set; }

        public DateTime? SuspendTs { get; set; }

        public virtual IEnumerable<SessionDbo> Sessions { get; set; }
        public virtual IEnumerable<ClaimDbo> Claims { get; set; }
    }
}
