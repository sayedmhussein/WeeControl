using System.Collections.Immutable;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Dicts
{
    public interface IClaimDicts
    {
        ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; }

        ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; }
    }
}
