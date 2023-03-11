using System;

namespace WeeControl.Core.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string reason) : base(reason)
    {
    }
}