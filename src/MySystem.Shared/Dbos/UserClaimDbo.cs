using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Entities;

namespace Sayed.MySystem.Shared.Dbos
{
    public class EmployeeClaimDbo : UserClaimBase
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
