using System;

namespace WeeControl.Backend.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base()
    {
    }

    public NotFoundException(string reason) : base(reason)
    {
    }

    [Obsolete(message:"Not necessary, put your message directly.")]
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}