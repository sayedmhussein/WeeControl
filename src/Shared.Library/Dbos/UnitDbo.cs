using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbos
{
    public class UnitDbo : UnitBase, IDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual BuildingDbo Building { get; set; }
    }
}
