using System;
using System.Collections.Generic;
using System.Linq;
using WeeControl.SharedKernel.Configurations;
using WeeControl.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.SharedKernel.EntityGroups.Territory.Interfaces;

namespace WeeControl.SharedKernel.EntityGroups.Territory.Attributes
{
    public class TerritoryAttribute : AppSettings, ITerritoryAttribute
    {
        private readonly Dictionary<CountryEnum, string> countries;

        public TerritoryAttribute()
        {
            countries = new Dictionary<CountryEnum, string>();
            foreach (var e in Enum.GetValues(typeof(CountryEnum)).Cast<CountryEnum>())
            {
                string value = json.Country[e.ToString()];
                countries.Add(e, value);
            }
        }

        public string GetCountryName(CountryEnum country)
        {
            return countries[country];
        }
    }
}
