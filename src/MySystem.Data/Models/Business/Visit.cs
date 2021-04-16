using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Business;
using MySystem.Data.Models.People;

namespace MySystem.Data.Models.Business
{
    [Table(nameof(Visit), Schema = nameof(Business))]
    [Comment("-")]
    public class Visit
    {
        [Key]
        public Guid VisitId { get; set; }

        public Guid ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public bool IsScheduled { get; set; }

        [Required, StringLength(3)]
        public string VisitType { get; set; }

        public DateTime VisitTs { get; set; }

        public TimeSpan Duration { get; set; }

        public Guid? VisitorId { get; set; }
        public virtual Employee Visitor { get; set; }
    }
}
