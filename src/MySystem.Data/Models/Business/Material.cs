using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Component;

namespace MySystem.Data.Models.Business
{
    [Table(nameof(Material), Schema = nameof(Business))]
    [Comment("-")]
    public class Material
    {
        public Guid MaterialId { get; set; }

        public Guid ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int Qty { get; set; }
    }
}
