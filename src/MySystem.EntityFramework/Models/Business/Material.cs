using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.EntityFramework.Models.Component;
using Sayed.MySystem.Shared.Dbos;

namespace Sayed.MySystem.EntityFramework.Models.Business
{
    [Table(nameof(Material), Schema = nameof(Business))]
    [Comment("-")]
    internal class Material
    {
        public Guid MaterialId { get; set; }

        public Guid ContractId { get; set; }
        public virtual ContractDbo Contract { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int Qty { get; set; }
    }
}
