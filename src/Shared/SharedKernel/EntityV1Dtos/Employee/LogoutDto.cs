using System;
using System.Collections.Generic;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    public class LogoutDto : IRequestDto
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
