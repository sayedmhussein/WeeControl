using System.Collections;
using System.Collections.Generic;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;

namespace MySystem.Application.Test.Territory.Query.GetTerritories
{
    public class AuthorizationScenariosTestData : IEnumerable<object[]>
    {
        private readonly IValuesService values = new ValueService();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read], false };
            yield return new object[] { values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Add], true };
            yield return new object[] { values.ClaimType[ClaimTypeEnum.Territory], values.ClaimTag[ClaimTagEnum.Read], true };
            yield return new object[] { "", values.ClaimTag[ClaimTagEnum.Read], true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
