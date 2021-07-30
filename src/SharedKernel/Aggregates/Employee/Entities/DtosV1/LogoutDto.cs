using System;
using System.Collections.Generic;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1
{
    public class LogoutDto : IRequestDto
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
