using System;

namespace WeeControl.Core.Application.Exceptions;

public class ConflictFailureException : Exception
{
    public ConflictFailureException()
    {
    }

    public ConflictFailureException(string msg) : base(msg)
    {
    }
}