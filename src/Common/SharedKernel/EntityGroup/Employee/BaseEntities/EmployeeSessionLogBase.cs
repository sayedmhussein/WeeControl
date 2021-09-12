using System;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroup.Employee.BaseEntities
{
    public abstract class BaseEmployeeSessionLog : IVerifyable
    {
        public Guid SessionId { get; set; }

        public DateTime LogTs { get; set; } = DateTime.Now;

        public string LogDetails { get; set; }
    }
}
