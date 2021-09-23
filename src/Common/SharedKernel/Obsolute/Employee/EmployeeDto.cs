using System;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Employee;

namespace WeeControl.Common.SharedKernel.Obsolute.Employee
{
    public class EmployeeDto : BaseEmployee, IDataTransferObject
    {
        public Guid? Id { get; set; }
    }
}
