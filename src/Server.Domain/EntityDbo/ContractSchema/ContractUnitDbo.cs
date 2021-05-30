using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interface;
using MySystem.SharedKernel.EntityBase;
using MySystem.Domain.EntityDbo.UnitSchema;

namespace MySystem.Domain.EntityDbo.ContractSchema
{
    public class ContractUnitDbo : ContractUnitBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }


        public virtual ContractDbo Contract { get; set; }

        public virtual UnitDbo Unit { get; set; }
    }
}
