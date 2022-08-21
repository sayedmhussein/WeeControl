using System;

namespace WeeControl.ApiApp.Application.Exceptions;

public class DeleteFailureException : Exception
{
    public DeleteFailureException(string message) : base(message)
    {
    }
}