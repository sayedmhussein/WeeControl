using System;
using MySystem.SharedKernel.EntityBases.Employee;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    public class EmployeeIdentityDto : EmployeeIdentityBase, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
