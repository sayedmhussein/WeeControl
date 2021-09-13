using System.Collections;
using System.Collections.Generic;
using WeeControl.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.EntityGroups.Employee.Interfaces;

namespace WeeControl.Backend.Application.Test.EntityGroup.Territory.V1.Queries
{
    public class GetTerritoriesHandlerTestersData : IEnumerable<object[]>
    {
        private readonly IEmployeeAttribute values = new EmployeeAppSetting();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { values.GetClaimType(ClaimTypeEnum.HumanResources), values.GetClaimTag(ClaimTagEnum.Read), false };
            yield return new object[] { values.GetClaimType(ClaimTypeEnum.HumanResources), values.GetClaimTag(ClaimTagEnum.Add), true };
            yield return new object[] { values.GetClaimType(ClaimTypeEnum.Territory), values.GetClaimTag(ClaimTagEnum.Read), true };
            yield return new object[] { "", values.GetClaimTag(ClaimTagEnum.Read), true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
