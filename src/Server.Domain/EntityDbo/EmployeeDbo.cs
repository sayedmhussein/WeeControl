using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interface;
using MySystem.SharedKernel.EntityBase;

namespace MySystem.Domain.EntityDbo
{
    public class EmployeeDbo : EmployeeBase, IEntityDbo
    {
        public static IEnumerable<EmployeeDbo> InitializeList(Guid officeid)
        {
            var sayed = new EmployeeDbo() { Id = Guid.NewGuid(), Title = "Mr.", FirstName = "Sayed", LastName = "Hussein", Gender = "m", OfficeId = officeid, Username = "username", Password = "password" };
            var hatem = new EmployeeDbo() { Id = Guid.NewGuid(), Title = "Mr.", FirstName = "Hatem", LastName = "Nagaty", Gender = "m", OfficeId = officeid, Username = "hatem", Password = "hatem", AccountLockArgument = "Blocked" };
            return new List<EmployeeDbo>()
            {
                sayed,
                hatem,
                new EmployeeDbo() { FirstName = "Yasser", LastName = "Gamal", OfficeId = officeid, Supervisor = sayed, Username = "sayed1", Password = "sayed" }, //
                new EmployeeDbo() { FirstName = "Yasser", LastName = "Gamal", OfficeId = officeid, SupervisorId = sayed.Id, Username = "sayed2", Password = "sayed" } //, SupervisorId = sayed.Id
            };
        }

        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Supervisor { get; set; }
        public virtual OfficeDbo Office { get; set; }
    }
}
