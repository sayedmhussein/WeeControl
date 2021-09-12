using System;
using WeeControl.SharedKernel.EntityGroup.Employee.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroup.Employee.DtosV1
{
    public class EmployeeDto : BaseEmployee, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
