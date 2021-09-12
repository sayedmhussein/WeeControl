using System;
namespace WeeControl.Backend.Application.Common.Exceptions
{
    public class ConflictFailureException : Exception
    {
        public ConflictFailureException() : base()
        {
        }

        public ConflictFailureException(string msg) : base(msg)
        {
        }
    }
}
