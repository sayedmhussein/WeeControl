using System;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects
{
    public record SessionLog
    {
        public DateTime Timestamp { get; set; }

        public string Details { get; set; }
    }
}