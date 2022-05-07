using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects.EssentialsObjects
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

        public static UserDbo Create(string email, string username, string password)
        {
            return new UserDbo() { Email = email, Username = username, Password = password };
        }
    }
}
