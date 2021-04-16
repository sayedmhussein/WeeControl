using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Component;

namespace MySystem.Data.Models.Business
{
    [Table(nameof(ContractUnit), Schema = nameof(Business))]
    [Index(nameof(ContractId), nameof(UnitId), IsUnique = false)]
    [Comment("-")]
    public class ContractUnit
    {
        [Key]
        public Guid ContractUnitId { get; set; }

        public Guid ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        public DateTime ActivationTs { get; set; }

        public DateTime CancellationTs { get; set; }
    }
}
