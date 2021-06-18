using System.Collections.Immutable;
using WeeControl.SharedKernel.BasicSchemas.Territory.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Territory.Interfaces
{
    public interface ITerritoryDicts
    {
        ImmutableDictionary<CountryEnum, string> Country { get; }
    }
}
