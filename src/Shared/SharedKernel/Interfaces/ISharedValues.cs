using System.Collections.Immutable;
using MySystem.SharedKernel.Enumerators;

namespace MySystem.SharedKernel.Interfaces
{
    public interface ISharedValues
    {
        ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; }

        ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; }

        ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; }

        ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; }

        ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; }

        ImmutableDictionary<CountryEnum, string> Country { get; }
    }
}