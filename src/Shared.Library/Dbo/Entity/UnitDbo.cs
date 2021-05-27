using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbo.Entity
{
    public class UnitDbo : UnitBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual BuildingDbo Building { get; set; }
    }
}
