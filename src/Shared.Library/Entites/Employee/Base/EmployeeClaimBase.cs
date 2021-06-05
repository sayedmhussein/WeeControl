using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.SharedKernel.Entities.Employee.Base
{
    public class EmployeeClaimBase
    {
        [Required]
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public DateTime GrantedTs { get; set; }

        public Guid GrantedById { get; set; }

        public DateTime? RevokedTs { get; set; }

        public Guid? RevokedById { get; set; }
    }

    [Flags]
    public enum ClaimTypes
    {
        Session,
        Office


    }

    [Flags]
    public enum ClaimTags
    {
        Add, Edit, Delete, Read
    }
}
