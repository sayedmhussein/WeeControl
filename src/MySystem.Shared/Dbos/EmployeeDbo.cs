using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.Shared.Dbos
{
    public class EmployeeDbo : EmployeeBase
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Supervisor { get; set; }
        public virtual OfficeDbo Office { get; set; }
    }
}
