using WeeControl.Common.SharedKernel.EntityGroups.Territory.Enums;

namespace WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces
{
    public interface ITerritoryAttribute
    {
        string GetCountryName(CountryEnum country);
    }
}
