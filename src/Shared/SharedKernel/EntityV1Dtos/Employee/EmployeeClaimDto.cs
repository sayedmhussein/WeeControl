using System;
using MySystem.SharedKernel.EntityBases.Employee;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    public class EmployeeClaimDto : EmployeeClaimBase, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
