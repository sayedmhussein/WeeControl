using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Entities;

namespace Sayed.MySystem.Shared.Dbos
{
    public class ContractUnitDbo : ContractUnitBase
    {
        [Key]
        public Guid ContractUnitId { get; set; }


        public virtual ContractDbo Contract { get; set; }


        public virtual UnitDbo Unit { get; set; }
    }
}
