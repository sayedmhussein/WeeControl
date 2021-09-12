using System;
namespace WeeControl.Backend.Application.Common.Exceptions
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
