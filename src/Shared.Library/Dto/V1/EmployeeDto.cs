using System;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.SharedKernel.Dto.V1
{
    public class EmployeeDto : EmployeeBase, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
