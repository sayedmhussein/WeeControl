using System;

namespace WeeControl.Backend.Application.Exceptions
{
    public class NotAllowedException : Exception
    {
        public NotAllowedException() : base()
        {
        }

        public NotAllowedException(string msg) : base(msg)
        {
        }
    }
}
