using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interface;
using MySystem.SharedKernel.EntityBase;

namespace MySystem.Domain.EntityDbo
{
    public class ContractUnitDbo : ContractUnitBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }


        public virtual ContractDbo Contract { get; set; }


        public virtual UnitDbo Unit { get; set; }
    }
}
