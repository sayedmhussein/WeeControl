using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Authorization.Bases
{
    public class UserClaimBase
    {
        [Required]
        [StringLength(5)]
        public string ClaimType { get; set; }
        
        public string ClaimValue { get; set; }

        public DateTime GrantedTs { get; set; }

        public Guid GrantedBy { get; set; }

        public DateTime? RevokedTs { get; set; }

        public Guid RevokedBy { get; set; }
    }
}