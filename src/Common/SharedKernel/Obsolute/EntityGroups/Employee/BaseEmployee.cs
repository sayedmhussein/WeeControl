using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Employee.Enums;

namespace WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Employee
{
    public abstract class BaseEmployee : IVerifyable
    {
        [StringLength(10, ErrorMessage = "Always use english common titles not exceeding 10 characters.")]
        public string EmployeeTitle { get; set; }

        

        [StringLength(1, ErrorMessage = "Use either m or f or keep it null.")]
        public string Gender { get; set; }

        

        [MaxLength(45)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(45)]
        [Phone]
        public string Mobile { get; set; }

        [MaxLength(45)]
        public string PhotoUrl { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Username length must be between 3 and 45 characher")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Username { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Maximum password length is 45 character.")]
        public string Password { get; set; }

        public PositionEnum EmployeePosition { get; set; }

        public DepartmentEnum EmployeeDepartment { get; set; }

        public bool IsProductive { get; set; }

        public Guid? ReportToId { get; set; }

        public Guid TerritoryId { get; set; }

        public string AccountLockArgument { get; set; }
    }
}
