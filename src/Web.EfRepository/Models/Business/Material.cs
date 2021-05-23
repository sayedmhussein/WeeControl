using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;
using MySystem.Web.EfRepository.Models.Component;

namespace MySystem.Web.EfRepository.Models.Business
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
