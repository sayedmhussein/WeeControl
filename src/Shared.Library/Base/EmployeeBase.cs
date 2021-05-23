using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Shared.Library.Base
{
    public abstract class EmployeeBase : PersonBase
    {
        [Required]
        [StringLength(45)]
        public string Username { get; set; }

        [Required]
        [StringLength(45)]
        public string Password { get; set; }

        public Positions Position { get; set; }

        public Departments Department { get; set; }

        public bool IsProductive { get; set; }

        public Guid? SupervisorId { get; set; }

        public Guid OfficeId { get; set; }

        public string AccountLockArgument { get; set; }

        public enum Positions
        {
            Helper = 0,
            Temporary = 1,
            Labor = 2,
            Technician = 3,
            SeniorTechnician = 4,
            Forman = 5,
            Supervisor = 6,
            Engineer = 7,
            SeniorEngineer = 8
        }

        public enum Departments
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
