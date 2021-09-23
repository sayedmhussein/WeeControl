using System;
using Microsoft.EntityFrameworkCore;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects
{
    [Owned]
    public record SessionLog(string Details)
    {
        public DateTime LogTs { get; set; }
    }
}