using System;
using System.Collections.Immutable;
using MySystem.SharedKernel.Enumerators.Employee;

namespace MySystem.SharedKernel.Interfaces.Values
{
    public interface IEmployeeValues
    {
        ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; }

        ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; }

        ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; }

        ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; }

        ImmutableDictionary<IdentityTypeEnum, string> IdentityType { get; }
    }
}
