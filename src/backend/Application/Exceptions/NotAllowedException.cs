using System;

namespace WeeControl.Core.Application.Exceptions;

public class NotAllowedException : Exception
{
    public NotAllowedException(string msg) : base(msg)
    {
    }
}