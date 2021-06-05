using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Entities.Territory.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.PublicSchema
{
    public class TerritoryDbo : TerritoryBase, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual TerritoryDbo ReportTo { get; set; }
        public ICollection<TerritoryDbo> ReportingFrom { get; set; }

        public TerritoryDbo()
        {
        }

        public TerritoryDbo(string name, string country) : this()
        {
            Name = name.ToUpper();
            CountryId = country.ToUpper();
        }

        public TerritoryDbo(string name, string country, Guid parentid) : this(name, country)
        {
            ReportToId = parentid;
        }
    }
}
