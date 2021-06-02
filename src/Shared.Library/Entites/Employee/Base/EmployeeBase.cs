using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.SharedKernel.Entities.Employee.Base
{
    public abstract class EmployeeBase
    {
        [StringLength(10, ErrorMessage = "Always use english common titles not exceeding 10 characters.")]
        public string EmployeeTitle { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(45)]
        public string SecondName { get; set; }

        [StringLength(45)]
        public string ThirdName { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(1, ErrorMessage = "Use either m or f or keep it null.")]
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(3)]
        public string Nationality { get; set; }

        [StringLength(3)]
        public string Language { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(50)]
        [Phone]
        public string Mobile { get; set; }

        [MaxLength(50)]
        public string PhotoUrl { get; set; }

        [Required]
        [StringLength(45)]
        public string Username { get; set; }

        [Required]
        [StringLength(45)]
        public string Password { get; set; }

        public Position EmployeePosition { get; set; }

        public Department EmployeeDepartment { get; set; }

        public bool IsProductive { get; set; }

        public Guid? ReportToId { get; set; }

        public Guid TerritoryId { get; set; }

        public string AccountLockArgument { get; set; }

        public enum Position
        {
            Helper = 0,
            Temporary = 1,
            Labor = 2,
            Technician = 3,
            SeniorTechnician = 4,
            Forman = 5,
            Supervisor = 6,
            Engineer = 7,
            SeniorEngineer = 8,

            Adminstrator,
            AdminstratorManager,

            HumanResourcesOfficer,
            SeniorHumanResourcesOfficer,

            Officer,
            AssistanceOfficer,
            CheifOfficer,
        };

        public enum Department
        {
            Finance = 1,
            Logistics = 2,
            Field = 3,
            HumanResources = 4,
            Estimation = 5,
            Sales = 6
        }
    }
}
