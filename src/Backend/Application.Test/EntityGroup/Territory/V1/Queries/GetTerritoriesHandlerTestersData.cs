using System.Collections;
using System.Collections.Generic;
using WeeControl.SharedKernel.EntityGroup.Employee;
using WeeControl.SharedKernel.EntityGroup.Employee.Enums;

namespace WeeControl.Backend.Application.Test.Territory.V1.Queries
{
    public class GetTerritoriesHandlerTestersData : IEnumerable<object[]>
    {
        private readonly IEmployeeLists values = new EmployeeLists();

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
