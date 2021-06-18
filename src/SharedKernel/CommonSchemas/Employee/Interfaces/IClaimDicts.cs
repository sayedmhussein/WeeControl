using System.Collections.Immutable;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.Dicts
{
    public interface IClaimDicts
    {
        ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; }

        ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; }
    }
}
