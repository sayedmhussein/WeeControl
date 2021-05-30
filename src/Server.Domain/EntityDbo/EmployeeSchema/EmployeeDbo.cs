using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interface;
using MySystem.SharedKernel.EntityBase;
using MySystem.Domain.EntityDbo.PublicSchema;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeDbo : EmployeeBase, IEntityDbo
    {
        public static IEnumerable<EmployeeDbo> InitializeList(Guid officeid)
        {
            var sayed = new EmployeeDbo() { Id = Guid.NewGuid(), Title = "Mr.", FirstName = "Sayed", LastName = "Hussein", Gender = "m", TerritoryId = officeid, Username = "username", Password = "password" };
            var hatem = new EmployeeDbo() { Id = Guid.NewGuid(), Title = "Mr.", FirstName = "Hatem", LastName = "Nagaty", Gender = "m", TerritoryId = officeid, Username = "hatem", Password = "hatem", AccountLockArgument = "Blocked" };
            return new List<EmployeeDbo>()
            {
                sayed,
                hatem,
                new EmployeeDbo() { FirstName = "Yasser", LastName = "Gamal", TerritoryId = officeid, ReportTo = sayed, Username = "sayed1", Password = "sayed" }, //
                new EmployeeDbo() { FirstName = "Yasser", LastName = "Gamal", TerritoryId = officeid, ReportToId = sayed.Id, Username = "sayed2", Password = "sayed" } //, SupervisorId = sayed.Id
            };
        }

        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo ReportTo { get; set; }
        public virtual TerritoryDbo Territory { get; set; }

        public virtual ICollection<EmployeeClaimDbo> Claims { get; set; }
        public virtual ICollection<EmployeeIdentityDbo> Identities { get; set; }
        public virtual ICollection<EmployeeSessionDbo> Sessions { get; set; }
    }
}
