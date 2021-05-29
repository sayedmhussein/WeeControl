using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interface;
using MySystem.SharedKernel.EntityBase;

namespace MySystem.Domain.EntityDbo
{
    public class ContractDbo : ContractBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ContractDbo Parent { get; set; }
        public ICollection<ContractDbo> Children { get; set; }


        public virtual EmployeeDbo Sales { get; set; }
        public virtual OfficeDbo Office { get; set; }
    }
}
