using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbos
{
    public class EmployeeSessionDbo : SessionBase
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
