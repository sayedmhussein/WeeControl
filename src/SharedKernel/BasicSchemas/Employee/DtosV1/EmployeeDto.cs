using System;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Bases;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1
{
    public class EmployeeDto : BaseEmployee, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
