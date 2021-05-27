using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;
using MySystem.Shared.Library.Definition;

namespace MySystem.Shared.Library.Dbo.Entity
{
    public class OfficeDbo : OfficeBase, IEntityDbo
    {
        public static IEnumerable<OfficeDbo> InitializeList()
        {
            var headOffice = new OfficeDbo()
            {
                Id = Guid.NewGuid(),

                CountryId = Country.USA,
                OfficeName = "Head Office in USA"
            };

            var saudiaHeadOffice = new OfficeDbo()
            {
                Id = Guid.NewGuid(),
                Parent = headOffice,

                CountryId = Country.SaudiArabia,
                OfficeName = "Saudia Head Office"
            };

            var egyptHeadOffice = new OfficeDbo()
            {
                Id = Guid.NewGuid(),
                Parent = headOffice,

                CountryId = Country.Egypt,
                OfficeName = "Egypt Head Office"
            };

            return new List<OfficeDbo>()
            {
                headOffice,
                saudiaHeadOffice,
                egyptHeadOffice,

                new OfficeDbo("Jeddah", Country.SaudiArabia, parentid: saudiaHeadOffice.Id),
                new OfficeDbo("Riyadh", Country.SaudiArabia, saudiaHeadOffice.Id),
                new OfficeDbo("Mecca", Country.SaudiArabia, saudiaHeadOffice.Id)
            };
        }

        [Key]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }
        public virtual OfficeDbo Parent { get; set; }

        public OfficeDbo()
        {
        }

        public OfficeDbo(string name, string country) : this()
        {
            OfficeName = name;
            CountryId = country;
        }

        public OfficeDbo(string name, string country, Guid parentid) : this(name, country)
        {
            ParentId = parentid;
        }
    }
}
