using System;
using System.Collections.Generic;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Employee.V1Dto
{
    public class LogoutDto : IDto
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
