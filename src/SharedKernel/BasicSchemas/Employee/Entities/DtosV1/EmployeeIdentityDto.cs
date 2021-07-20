using System;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Entities;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Entities.DtosV1
{
    public class EmployeeIdentityDto : BaseEmployeeIdentity, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
