using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.EntityFramework.Models.Business;
using Sayed.MySystem.EntityFramework.Models.Component;
using Sayed.MySystem.EntityFramework.Models.People;
using Sayed.MySystem.Shared.Dbos;

namespace Sayed.MySystem.EntityFramework.Models.Business
{
    [Table(nameof(Visit), Schema = nameof(Business))]
    [Comment("-")]
    internal class Visit
    {
        [Key]
        public Guid VisitId { get; set; }

        public Guid ContractId { get; set; }
        public virtual ContractDbo Contract { get; set; }

        public Guid UnitId { get; set; }
        public virtual UnitDbo Unit { get; set; }

        public bool IsScheduled { get; set; }

        [Required, StringLength(3)]
        public string VisitType { get; set; }

        public DateTime VisitTs { get; set; }

        public TimeSpan Duration { get; set; }

        public Guid? VisitorId { get; set; }
        public virtual EmployeeDbo Visitor { get; set; }
    }
}
