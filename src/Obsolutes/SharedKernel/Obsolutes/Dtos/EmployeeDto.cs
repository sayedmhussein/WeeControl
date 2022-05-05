using System;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Obsolutes.Dtos
{
    public class EmployeeDto : BaseEmployee, IDataTransferObject
    {
        public Guid? Id { get; set; }
    }
}
