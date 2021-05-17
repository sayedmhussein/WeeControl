using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.Shared.Dbos
{
    public class EmployeeSessionDbo : SessionBase
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
