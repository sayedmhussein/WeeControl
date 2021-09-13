using System;
using WeeControl.SharedKernel.EntityGroups.Employee;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1.Employee
{
    public class EmployeeDto : BaseEmployee, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
