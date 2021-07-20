using System;
using System.Collections.Generic;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Entities.DtosV1
{
    public class LogoutDto : IRequestDto
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
