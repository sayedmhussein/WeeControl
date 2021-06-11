using System;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    public class RefreshLoginDto : IRequestDto
    {
        public RequestMetadata Metadata { get; set; }
    }
}

