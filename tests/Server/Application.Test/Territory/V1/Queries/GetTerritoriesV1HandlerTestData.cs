using System.Collections;
using System.Collections.Generic;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.Server.Application.Test.Territory.V1.Queries
{
    public class GetTerritoriesV1HandlerTestData : IEnumerable<object[]>
    {
        private readonly IClaimDicts values = new ClaimDicts();

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
