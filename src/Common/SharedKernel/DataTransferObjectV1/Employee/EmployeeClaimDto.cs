using System;
using WeeControl.Common.SharedKernel.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee
{
    public class EmployeeClaimDto : BaseEmployeeClaim, IDataTransferObject
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
