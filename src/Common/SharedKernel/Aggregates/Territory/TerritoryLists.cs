using System;
using System.Collections.Generic;
using System.Linq;
using WeeControl.SharedKernel.Aggregates.Territory.Enums;
using WeeControl.SharedKernel.Configurations;

namespace WeeControl.SharedKernel.Aggregates.Territory
{
    public class TerritoryLists : AppSettings, ITerritoryLists
    {
        private readonly Dictionary<CountryEnum, string> countries;

        public TerritoryLists()
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
