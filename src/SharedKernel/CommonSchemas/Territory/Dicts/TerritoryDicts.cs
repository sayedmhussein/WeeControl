using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WeeControl.SharedKernel.CommonSchemas.Common.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Territory.Enums;
using WeeControl.SharedKernel.CommonSchemas.Territory.Interfaces;

namespace WeeControl.SharedKernel.CommonSchemas.Territory.Dicts
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
