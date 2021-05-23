using System;
namespace MySystem.Shared.Library.Base
{
    public abstract class SessionLogBase
    {
        public Guid SessionId { get; set; }

        public DateTime LogTs { get; set; }
    }
}
