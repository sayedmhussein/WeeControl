using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects
{
    [Owned]
    public class Claims
    {
        [Required]
        [StringLength(5)]
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public DateTime GrantedTs { get; set; }

        public Employee GrantedBy { get; set; }

        public DateTime? RevokedTs { get; set; }

        public Employee RevokedBy { get; set; }
    }
}