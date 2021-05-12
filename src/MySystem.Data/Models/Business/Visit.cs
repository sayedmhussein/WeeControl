using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.ServerData.Models.Business;
using Sayed.MySystem.ServerData.Models.Component;
using Sayed.MySystem.ServerData.Models.People;

namespace Sayed.MySystem.ServerData.Models.Business
{
    [Table(nameof(Visit), Schema = nameof(Business))]
    [Comment("-")]
    internal class Visit
    {
        [Key]
        public Guid VisitId { get; set; }

        public Guid ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        public bool IsScheduled { get; set; }

        [Required, StringLength(3)]
        public string VisitType { get; set; }

        public DateTime VisitTs { get; set; }

        public TimeSpan Duration { get; set; }

        public Guid? VisitorId { get; set; }
        public virtual Employee Visitor { get; set; }
    }
}
