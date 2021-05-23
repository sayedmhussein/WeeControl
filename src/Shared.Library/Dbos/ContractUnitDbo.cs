using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbos
{
    public class ContractUnitDbo : ContractUnitBase
    {
        [Key]
        public Guid ContractUnitId { get; set; }


        public virtual ContractDbo Contract { get; set; }


        public virtual UnitDbo Unit { get; set; }
    }
}
