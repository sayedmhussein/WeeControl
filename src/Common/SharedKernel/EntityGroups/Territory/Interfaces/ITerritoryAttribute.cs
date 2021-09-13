using WeeControl.SharedKernel.EntityGroups.Territory.Enums;

namespace WeeControl.SharedKernel.EntityGroups.Territory.Interfaces
{
    public interface ITerritoryAttribute
    {
        string GetCountryName(CountryEnum country);
    }
}
