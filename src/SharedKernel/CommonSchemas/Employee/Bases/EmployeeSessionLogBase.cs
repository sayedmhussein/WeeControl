using System;
namespace WeeControl.SharedKernel.CommonSchemas.Employee.Bases
{
    public abstract class BaseEmployeeSessionLog
    {
        public Guid SessionId { get; set; }

        public DateTime LogTs { get; set; } = DateTime.Now;

        public string LogDetails { get; set; }
    }
}
