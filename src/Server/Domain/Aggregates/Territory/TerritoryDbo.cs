using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Aggregates.Territory.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Server.Domain.BasicDbos.Territory
{
    public class TerritoryDbo : BaseTerritory, IEntityDbo
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
