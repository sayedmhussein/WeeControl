using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sayed.MySystem.Shared.Entities;

namespace Sayed.MySystem.Shared.Dbos
{
    public class ContractDbo : ContractBase
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ContractDbo Parent { get; set; }
        public ICollection<ContractDbo> Children { get; set; }


        public virtual EmployeeDbo Sales { get; set; }
        public virtual OfficeDbo Office { get; set; }
    }
}
