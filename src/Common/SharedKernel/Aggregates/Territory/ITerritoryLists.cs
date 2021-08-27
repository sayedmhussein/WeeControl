using System;
using WeeControl.SharedKernel.Aggregates.Territory.Enums;

namespace WeeControl.SharedKernel.Aggregates.Territory
{
    public interface ITerritoryLists
    {
        string GetCountryName(CountryEnum country);
    }
}
