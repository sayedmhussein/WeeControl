using System.Collections.Immutable;
using WeeControl.SharedKernel.CommonSchemas.Territory.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Territory.Interfaces
{
    public interface ITerritoryDicts
    {
        ImmutableDictionary<CountryEnum, string> Country { get; }
    }
}
