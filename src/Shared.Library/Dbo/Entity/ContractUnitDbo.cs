using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbo.Entity
{
    public class ContractUnitDbo : ContractUnitBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }


        public virtual ContractDbo Contract { get; set; }


        public virtual UnitDbo Unit { get; set; }
    }
}
