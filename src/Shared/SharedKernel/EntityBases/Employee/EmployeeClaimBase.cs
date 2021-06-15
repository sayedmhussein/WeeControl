﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.SharedKernel.EntityBases.Employee
{
    public abstract class EmployeeClaimBase
    {
        [Required]
        [StringLength(5)]
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public DateTime GrantedTs { get; set; }

        public Guid GrantedById { get; set; }

        public DateTime? RevokedTs { get; set; }

        public Guid? RevokedById { get; set; }
    }
}
