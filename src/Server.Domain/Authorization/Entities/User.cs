using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.Common.Interfaces;
using WeeControl.SharedKernel.Authorization.Bases;

namespace WeeControl.Server.Domain.Authorization.Entities
{
    public class User : UserBase, IAggregateRoot
    {
        [Key]
        public Guid UserId { get; set; }

        public ICollection<UserSession> Sessions { get; set; }
        
        public virtual ICollection<UserClaim> Claims { get; set; }

        private User()
        {
        }

        public User(string username, string password) : this()
        {
            Username = username;
            Password = password;
        }
    }
}