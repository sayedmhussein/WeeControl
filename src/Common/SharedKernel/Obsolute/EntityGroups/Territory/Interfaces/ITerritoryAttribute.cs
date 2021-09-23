using WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Territory.Enums;

namespace WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Territory.Interfaces
{
    public interface ITerritoryAttribute
    {
        string GetCountryName(CountryEnum country);
    }
}
