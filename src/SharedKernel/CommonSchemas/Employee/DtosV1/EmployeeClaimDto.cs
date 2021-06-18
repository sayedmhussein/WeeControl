using System;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using WeeControl.SharedKernel.CommonSchemas.Employee.Bases;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.DtosV1
{
    public class EmployeeClaimDto : BaseEmployeeClaim, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
