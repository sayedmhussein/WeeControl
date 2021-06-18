using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Dicts
{
    public class ClaimDicts : BaseDicts, IClaimDicts
    {
        public ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; private set; }

        public ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; private set; }

        public ClaimDicts()
        {
            var claimType = new Dictionary<ClaimTypeEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ClaimTypeEnum)).Cast<ClaimTypeEnum>())
            {
                string value = obj.ClaimType[e.ToString()];
                claimType.Add(e, value);
            }
            ClaimType = claimType.ToImmutableDictionary();

            var claimTag = new Dictionary<ClaimTagEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ClaimTagEnum)).Cast<ClaimTagEnum>())
            {
                string value = obj.ClaimTag[e.ToString()];
                claimTag.Add(e, value);
            }
            ClaimTag = claimTag.ToImmutableDictionary();
        }
    }
}
