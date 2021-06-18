using System;
using System.Collections.Generic;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.DtosV1
{
    public class LogoutDto : IRequestDto
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
