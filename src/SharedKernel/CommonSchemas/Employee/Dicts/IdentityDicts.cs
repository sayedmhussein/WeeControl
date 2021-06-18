using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WeeControl.SharedKernel.CommonSchemas.Common.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.Dicts
{
    public class IdentityDicts : BaseDicts, IIdentityDicts
    {
        public ImmutableDictionary<IdentityTypeEnum, string> IdentityType { get; private set; }

        public IdentityDicts()
        {
            var identityType = new Dictionary<IdentityTypeEnum, string>();
            foreach (var e in Enum.GetValues(typeof(IdentityTypeEnum)).Cast<IdentityTypeEnum>())
            {
                string value = obj.IdentityType[e.ToString()];
                identityType.Add(e, value);
            }
            IdentityType = identityType.ToImmutableDictionary();
        }
    }
}
