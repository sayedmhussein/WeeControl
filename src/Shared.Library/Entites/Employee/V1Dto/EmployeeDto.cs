using System;
using MySystem.SharedKernel.Entities.Employee.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Employee.V1Dto
{
    public class EmployeeDto : EmployeeBase, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
