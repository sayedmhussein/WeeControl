using System;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1
{
    public class EmployeeClaimDto : BaseEmployeeClaim, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
