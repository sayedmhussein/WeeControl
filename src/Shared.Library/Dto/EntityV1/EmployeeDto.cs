using System;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dto.EntityV1
{
    public class EmployeeDto : EmployeeBase, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
