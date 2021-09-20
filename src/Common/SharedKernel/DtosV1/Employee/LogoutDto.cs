using System;
using System.Collections.Generic;

namespace WeeControl.Common.SharedKernel.DtosV1.Employee
{
    public class LogoutDto
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
