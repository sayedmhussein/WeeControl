using System;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Obsolutes.Dtos
{
    public class EmployeeClaimDto : BaseEmployeeClaim, IDataTransferObject
    {
        public Guid? Id { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
