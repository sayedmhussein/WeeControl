using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.Domain.EntityDbo
{
    public class EmployeeClaimDbo : UserClaimBase, IEntityDbo
    {
        public static IEnumerable<EmployeeClaimDbo> InitializeList(Guid employeeid)
        {
            return new List<EmployeeClaimDbo>()
            {
                new EmployeeClaimDbo() { EmployeeId = employeeid, ClaimType = "admin", ClaimValue = "admin" }
            };
        }

        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Employee { get; set; }
    }
}
