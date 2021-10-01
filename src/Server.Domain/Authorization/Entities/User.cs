using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Authorization.Bases;

namespace WeeControl.Server.Domain.Authorization.Entities
{
    public class User : UserBase
    {
        [Key]
        public Guid UserId { get; set; }
        
        public string TerritoryCode { get; set; }

        public ICollection<UserSession> Sessions { get; set; }
        
        public ICollection<UserClaim> Claims { get; set; }
    }
}