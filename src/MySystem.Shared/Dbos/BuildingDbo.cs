using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.Shared.Dbos
{
    public class BuildingDbo : BuildingBase
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
