using System;
using WeeControl.SharedKernel.EntityGroup.Territory.Enums;

namespace WeeControl.SharedKernel.EntityGroup.Territory
{
    public interface ITerritoryLists
    {
        string GetCountryName(CountryEnum country);
    }
}
