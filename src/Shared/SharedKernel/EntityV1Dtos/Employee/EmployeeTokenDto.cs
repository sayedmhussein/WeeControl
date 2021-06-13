using System;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    [Obsolete("to remove IRequestDto interface and use only IDto")]
    public class EmployeeTokenDto : IRequestDto
    {
        public string Token { get; set; }

        //Todo: to remove this interface and use only IDto
        public RequestMetadata Metadata { get; set; }
    }
}
