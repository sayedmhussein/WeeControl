using System;

namespace WeeControl.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string reason) : base(reason)
    {
    }
}