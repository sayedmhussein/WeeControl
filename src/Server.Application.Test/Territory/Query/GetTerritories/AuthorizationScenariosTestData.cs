using System;
using System.Collections;
using System.Collections.Generic;
using MySystem.SharedKernel.Entities.Public.Constants;

namespace MySystem.Application.Test.Territory.Query.GetTerritories
{
    public class AuthorizationScenariosTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Claims.Types[Claims.ClaimType.HumanResources], Claims.Tags[Claims.ClaimTag.Read], false };
            yield return new object[] { Claims.Types[Claims.ClaimType.Office], Claims.Tags[Claims.ClaimTag.Read], true };
            yield return new object[] { Claims.Types[Claims.ClaimType.HumanResources], Claims.Tags[Claims.ClaimTag.Add], true };
            yield return new object[] { "", Claims.Tags[Claims.ClaimTag.Read], true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
