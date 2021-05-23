using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbos
{
    public class EmployeeDbo : EmployeeBase
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Supervisor { get; set; }
        public virtual OfficeDbo Office { get; set; }
    }
}
