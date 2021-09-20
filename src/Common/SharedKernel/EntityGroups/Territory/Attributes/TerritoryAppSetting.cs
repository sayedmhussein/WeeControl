using System.Collections.Generic;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;
using WeeControl.Common.SharedKernel.Services;

namespace WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes
{
    public class TerritoryAppSetting : ITerritoryAttribute
    {
        private readonly AppSettingReader appSettingReader;
        private Dictionary<CountryEnum, string> countries;

        public TerritoryAppSetting()
        {
            appSettingReader = new AppSettingReader(typeof(TerritoryAppSetting).Namespace, "attributes.json");
        }

        public string GetCountryName(CountryEnum country)
        {
            appSettingReader.PopulateAttribute(ref countries, "Countries");
            return countries[country];
        }
    }
}
