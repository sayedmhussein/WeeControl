using System;
using Microsoft.EntityFrameworkCore;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects
{
    [Owned]
    public record SessionLog
    {
        public DateTime Timestamp { get; set; }

        public string Details { get; set; }
    }
}