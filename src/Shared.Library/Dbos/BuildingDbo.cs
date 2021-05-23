using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dbos
{
    public class BuildingDbo : BuildingBase, IDbo
    {
        [Key]
        public Guid Id { get; set; }

        public BuildingDbo()
        {
        }

        public BuildingDbo(string name, string country) : this()
        {
            BuildingName = name;
            CountryId = country;
        }
    }
}
