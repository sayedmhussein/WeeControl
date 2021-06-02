using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Entities.Employee.Base;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeClaimDbo : EmployeeClaimBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual EmployeeDbo Employee { get; set; }

        //[ForeignKey(nameof(GrantedById))]
        //public virtual EmployeeDbo GrantedBy { get; set; }

        //[ForeignKey(nameof(RevokedById))]
        //public virtual EmployeeDbo RevokedBy { get; set; }

        public static IEnumerable<EmployeeClaimDbo> InitializeList(Guid employeeid)
        {
            var claims = new List<EmployeeClaimDbo>();

            var tags = string.Join(';', Claims.Tags.Values);

            var types = Claims.Types.Values;
            foreach (var type in types)
            {
                claims.Add(new EmployeeClaimDbo()
                {
                    ClaimType = type,
                    ClaimValue = tags,
                    EmployeeId = employeeid,
                    GrantedById = employeeid
                });
            }

            return claims;
        }
    }
}
