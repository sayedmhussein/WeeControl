using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using MySystem.SharedKernel.Enumerators.Territory;
using MySystem.SharedKernel.Interfaces.Values;

namespace MySystem.SharedKernel.Services
{
    public class TerritoryValues : ITerritoryValues
    {
        public ImmutableDictionary<CountryEnum, string> Country { get; private set; }

        public TerritoryValues()
        {
            dynamic obj = new EmbeddedResourcesManager(
                Assembly.GetExecutingAssembly())
                .GetSerializedAsJson("MySystem.SharedKernel.Configuration.appsettings.json");

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
