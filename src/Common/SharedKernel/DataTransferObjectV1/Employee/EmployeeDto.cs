using System;
using WeeControl.Common.SharedKernel.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee
{
    public class EmployeeDto : BaseEmployee, IDataTransferObject
    {
        public Guid? Id { get; set; }
    }
}
