using System;

namespace WeeControl.ApiApp.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string reason) : base(reason)
    {
    }
}