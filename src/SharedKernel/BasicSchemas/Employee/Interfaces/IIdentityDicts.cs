using System;
using System.Collections.Immutable;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Dicts
{
    public interface IIdentityDicts
    {
        ImmutableDictionary<IdentityTypeEnum, string> IdentityType { get; }
    }
}
