using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MySystem.ServerData.Models.Component
{
    [Table(nameof(Item), Schema = nameof(Component))]
    [Index(nameof(Partnumber), IsUnique = true)]
    [Index(nameof(Partname), IsUnique = false)]
    [Comment("-")]
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }

        public string Partnumber { get; set; }

        public string Partname { get; set; }

        public decimal Cost { get; set; }
    }
}
