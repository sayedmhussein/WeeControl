using System;
using WeeControl.SharedKernel.EntityGroup.Employee.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroup.Employee.DtosV1
{
    public class EmployeeClaimDto : BaseEmployeeClaim, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
