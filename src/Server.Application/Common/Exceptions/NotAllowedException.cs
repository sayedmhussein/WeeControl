using System;
namespace MySystem.Application.Common.Exceptions
{
    public class NotAllowedException : Exception
    {
        public NotAllowedException(string argument) : base(argument)
        {
        }
    }
}
