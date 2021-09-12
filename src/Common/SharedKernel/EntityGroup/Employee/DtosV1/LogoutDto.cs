using System;
using System.Collections.Generic;

namespace WeeControl.SharedKernel.EntityGroup.Employee.DtosV1
{
    public class LogoutDto
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
