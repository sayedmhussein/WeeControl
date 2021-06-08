using System.Collections.Immutable;
using MySystem.SharedKernel.Enumerators;

namespace MySystem.SharedKernel.Interfaces
{
    public interface ISharedValues
    {
        public ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; }

        public ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; }

        public ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; }

        public ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; }

        public ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; }

        public ImmutableDictionary<CountryEnum, string> Country { get; }
    }
}