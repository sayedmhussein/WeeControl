using System;
using Sayed.MySystem.Shared.Entities;

namespace Sayed.MySystem.Shared.Dtos.V1.Entities
{
    public class EmployeeDto : EmployeeBase
    {
        public Guid? Id { get; set; }
    }
}
