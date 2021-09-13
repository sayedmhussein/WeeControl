using System;
using System.Collections.Generic;

namespace WeeControl.SharedKernel.DtosV1.Employee
{
    public class LogoutDto
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
