using System;
using System.Collections.Generic;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    public class LogoutDto : IDto
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
