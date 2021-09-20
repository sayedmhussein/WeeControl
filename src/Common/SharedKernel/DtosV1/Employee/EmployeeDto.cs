using System;
using WeeControl.Common.SharedKernel.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Employee
{
    public class EmployeeDto : BaseEmployee, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
