using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.Shared.Dbos
{
    public class UnitDbo : UnitBase
    {
        [Key]
        public Guid UnitId { get; set; }

        public virtual BuildingDbo Building { get; set; }
    }
}
