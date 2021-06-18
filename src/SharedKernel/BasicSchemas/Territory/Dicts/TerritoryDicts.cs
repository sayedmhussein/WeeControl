using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Territory.Dicts
{
    public class TerritoryDicts : BaseDicts, ITerritoryDicts
    {
        public ImmutableDictionary<CountryEnum, string> Country { get; private set; }

        public TerritoryDicts()
        {
            var country = new Dictionary<CountryEnum, string>();
            foreach (var e in Enum.GetValues(typeof(CountryEnum)).Cast<CountryEnum>())
            {
                string value = obj.Country[e.ToString()];
                country.Add(e, value);
            }
            Country = country.ToImmutableDictionary();
        }
    }
}
