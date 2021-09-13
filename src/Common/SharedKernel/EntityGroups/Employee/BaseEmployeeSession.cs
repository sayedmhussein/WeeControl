﻿using System;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroups.Employee
{
    public abstract class BaseEmployeeSession : IVerifyable
    {
        public Guid EmployeeId { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; }

        public DateTime? TerminationTs { get; set; }
    }
}