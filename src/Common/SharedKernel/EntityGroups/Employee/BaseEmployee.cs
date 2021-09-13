using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroups.Employee
{
    public abstract class BaseEmployee : IVerifyable
    {
        [StringLength(10, ErrorMessage = "Always use english common titles not exceeding 10 characters.")]
        public string EmployeeTitle { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "First name cannot be less than 3 chars or longer than 45 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(45)]
        public string SecondName { get; set; }

        [StringLength(45)]
        public string ThirdName { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Last name cannot be less than 3 chars or longer than 45 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(1, ErrorMessage = "Use either m or f or keep it null.")]
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Nationality { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Language { get; set; }

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
