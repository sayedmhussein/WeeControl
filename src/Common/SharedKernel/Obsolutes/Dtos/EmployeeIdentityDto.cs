using System;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Obsolutes.Dtos
{
    public class EmployeeIdentityDto : BaseEmployeeIdentity, IDataTransferObject
    {
        public Guid? Id { get; set; }
    }
}
