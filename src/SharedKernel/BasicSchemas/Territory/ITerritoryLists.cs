using System;
using WeeControl.SharedKernel.BasicSchemas.Territory.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Territory
{
    public interface ITerritoryLists
    {
        string GetCountryName(CountryEnum country);
    }
}
