using System;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Employee;

namespace WeeControl.Common.SharedKernel.Obsolute.Employee
{
    public class EmployeeClaimDto : BaseEmployeeClaim, IDataTransferObject
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
