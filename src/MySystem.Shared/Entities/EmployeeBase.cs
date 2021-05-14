using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Dbos;

namespace Sayed.MySystem.Shared.Entities
{
    public abstract class EmployeeBase : PersonBase
    {
        public Guid? SupervisorId { get; set; }
        public virtual EmployeeBase Supervisor { get; set; }

        public Guid OfficeId { get; set; }
        public virtual OfficeDbo Office { get; set; }

        [StringLength(45)]
        public string Username { get; set; }

        [StringLength(45)]
        public string Password { get; set; }

        public int Position { get; set; }

        public int Department { get; set; }

        public bool IsProductive { get; set; }

        public string LockingReason { get; set; }
    }
}
