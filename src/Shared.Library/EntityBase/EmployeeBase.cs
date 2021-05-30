using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Definition;

namespace MySystem.SharedKernel.EntityBase
{
    public abstract class EmployeeBase : PersonBase
    {
        [Required]
        [StringLength(45)]
        public string Username { get; set; }

        [Required]
        [StringLength(45)]
        public string Password { get; set; }

        public EmployeePosition Position { get; set; }

        public OrganizationDepartment Department { get; set; }

        public bool IsProductive { get; set; }

        public Guid? ReportToId { get; set; }

        public Guid TerritoryId { get; set; }

        public string AccountLockArgument { get; set; }
    }
}
