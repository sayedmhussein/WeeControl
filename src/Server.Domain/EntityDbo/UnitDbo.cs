using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.Domain.EntityDbo
{
    public class UnitDbo : UnitBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual BuildingDbo Building { get; set; }
    }
}
