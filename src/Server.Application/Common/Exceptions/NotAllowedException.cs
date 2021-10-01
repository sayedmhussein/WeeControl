using System;

namespace WeeControl.Application.Common.Exceptions
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
