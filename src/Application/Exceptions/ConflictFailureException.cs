using System;

namespace WeeControl.Application.Exceptions;

public class ConflictFailureException : Exception
{
    public ConflictFailureException() : base()
    {
    }

    public ConflictFailureException(string msg) : base(msg)
    {
    }
}