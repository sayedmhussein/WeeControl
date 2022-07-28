using System;

namespace WeeControl.Application.Exceptions;

public class NotAllowedException : Exception
{
    public NotAllowedException(string msg) : base(msg)
    {
    }
}