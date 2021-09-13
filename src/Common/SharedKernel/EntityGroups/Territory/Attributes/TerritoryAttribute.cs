using System;
using System.Collections.Generic;
using System.Linq;
using WeeControl.SharedKernel.Configurations;
using WeeControl.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.SharedKernel.EntityGroups.Territory.Interfaces;
using WeeControl.SharedKernel.Helpers;

namespace WeeControl.SharedKernel.EntityGroups.Territory.Attributes
{
    public class TerritoryAttribute : AttributesReader, ITerritoryAttribute
    {
        private Dictionary<CountryEnum, string> countries;

        public TerritoryAttribute() : base(typeof(TerritoryAttribute).Namespace)
        {
        }

        public string GetCountryName(CountryEnum country)
        {
            PopulateDictionary(ref countries, "Countries");
            return countries[country];
        }
    }
}
