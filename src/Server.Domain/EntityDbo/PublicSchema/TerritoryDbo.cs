using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Definition;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.Domain.EntityDbo.PublicSchema
{
    public class TerritoryDbo : TerritoryBase, IEntityDbo
    {
        public static IEnumerable<TerritoryDbo> InitializeList()
        {
            var headOffice = new TerritoryDbo()
            {
                Id = Guid.NewGuid(),

                CountryId = Country.USA,
                OfficeName = "Head Office in USA"
            };

            var saudiaHeadOffice = new TerritoryDbo()
            {
                Id = Guid.NewGuid(),
                ReportTo = headOffice,

                CountryId = Country.SaudiArabia,
                OfficeName = "Saudia Head Office"
            };

            var egyptHeadOffice = new TerritoryDbo()
            {
                Id = Guid.NewGuid(),
                ReportTo = headOffice,

                CountryId = Country.Egypt,
                OfficeName = "Egypt Head Office"
            };

            return new List<TerritoryDbo>()
            {
                headOffice,
                saudiaHeadOffice,
                egyptHeadOffice,

                new TerritoryDbo("Jeddah", Country.SaudiArabia, parentid: saudiaHeadOffice.Id),
                new TerritoryDbo("Riyadh", Country.SaudiArabia, saudiaHeadOffice.Id),
                new TerritoryDbo("Mecca", Country.SaudiArabia, saudiaHeadOffice.Id)
            };
        }

        [Key]
        public Guid Id { get; set; }

        public virtual TerritoryDbo ReportTo { get; set; }

        public TerritoryDbo()
        {
        }

        public TerritoryDbo(string name, string country) : this()
        {
            OfficeName = name.ToUpper();
            CountryId = country.ToUpper();
        }

        public TerritoryDbo(string name, string country, Guid parentid) : this(name, country)
        {
            ReportToId = parentid;
        }
    }
}
