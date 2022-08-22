using System;

namespace WeeControl.ApiApp.Application.Exceptions;

public class NotAllowedException : Exception
{
    public NotAllowedException(string msg) : base(msg)
    {
    }
}