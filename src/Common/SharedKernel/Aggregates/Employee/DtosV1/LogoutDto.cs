using System;
using System.Collections.Generic;

namespace WeeControl.SharedKernel.Aggregates.Employee.DtosV1
{
    public class LogoutDto
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
