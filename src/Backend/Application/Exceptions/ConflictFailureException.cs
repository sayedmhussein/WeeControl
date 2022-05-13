using System;

namespace WeeControl.Backend.Application.Exceptions;

public class ConflictFailureException : Exception
{
    public ConflictFailureException() : base()
    {
    }

    public ConflictFailureException(string msg) : base(msg)
    {
    }
}