using System;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1
{
    public class EmployeeIdentityDto : BaseEmployeeIdentity, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
