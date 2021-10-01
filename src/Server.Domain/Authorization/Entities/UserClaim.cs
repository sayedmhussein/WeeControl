using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Authorization.Bases;

namespace WeeControl.Server.Domain.Authorization.Entities
{
    public class UserClaim : UserClaimBase
    {
        [Key]
        public Guid UserClaimId { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
        
    }
}