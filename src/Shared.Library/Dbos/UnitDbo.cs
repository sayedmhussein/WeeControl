using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbos
{
    public class UnitDbo : UnitBase
    {
        [Key]
        public Guid UnitId { get; set; }

        public virtual BuildingDbo Building { get; set; }
    }
}
