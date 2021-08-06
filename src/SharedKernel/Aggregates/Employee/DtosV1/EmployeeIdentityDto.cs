using System;
using WeeControl.SharedKernel.Aggregates.Employee.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.DtosV1
{
    public class EmployeeIdentityDto : BaseEmployeeIdentity, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
