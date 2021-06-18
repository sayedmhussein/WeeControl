using System;
using System.Collections.Immutable;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.Dicts
{
    public interface IIdentityDicts
    {
        ImmutableDictionary<IdentityTypeEnum, string> IdentityType { get; }
    }
}
