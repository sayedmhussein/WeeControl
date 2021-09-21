using System;
using WeeControl.Common.SharedKernel.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee
{
    public class EmployeeIdentityDto : BaseEmployeeIdentity, IDataTransferObject
    {
        public Guid? Id { get; set; }
    }
}
