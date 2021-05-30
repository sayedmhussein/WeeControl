using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.ContractSchema;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.EntityDbo.UnitSchema;

namespace MySystem.Persistence.Infrastructure.EfRepository.Models.Business
{
    [Obsolete]
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
