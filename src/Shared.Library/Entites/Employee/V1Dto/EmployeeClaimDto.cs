using System;
using MySystem.SharedKernel.Entities.Employee.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Employee.V1Dto
{
    public class EmployeeClaimDto : EmployeeClaimBase, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
