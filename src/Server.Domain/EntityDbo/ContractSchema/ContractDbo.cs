using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.EntityBase;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.EntityDbo.PublicSchema;

namespace MySystem.Domain.EntityDbo.ContractSchema
{
    public class ContractDbo : ContractBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ContractDbo Parent { get; set; }
        public ICollection<ContractDbo> Children { get; set; }


        public virtual EmployeeDbo Sales { get; set; }
        public virtual TerritoryDbo Office { get; set; }
    }
}
