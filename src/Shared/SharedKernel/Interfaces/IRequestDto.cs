using System;
using MySystem.SharedKernel.EntityV1Dtos.Common;

namespace MySystem.SharedKernel.Interfaces
{
    public interface IRequestDto
    {
        RequestMetadata Metadata { get; set; }
    }
}
