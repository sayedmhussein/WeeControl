using System;
namespace MySystem.SharedKernel.EntityBase
{
    public abstract class SessionLogBase
    {
        public Guid SessionId { get; set; }

        public DateTime LogTs { get; set; }

        public string LogDetails { get; set; }
    }
}
