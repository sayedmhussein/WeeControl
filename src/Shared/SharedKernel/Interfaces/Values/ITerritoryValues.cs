using System;
using System.Collections.Immutable;
using MySystem.SharedKernel.Enumerators.Territory;

namespace MySystem.SharedKernel.Interfaces.Values
{
    public interface ITerritoryValues
    {
        ImmutableDictionary<CountryEnum, string> Country { get; }
    }
}
