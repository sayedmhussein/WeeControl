using System;

namespace WeeControl.Core.Domain.Exceptions;

public class DomainOutOfRanceException : ArgumentOutOfRangeException
{
    public DomainOutOfRanceException(string arg) : base(arg)
    {
    }
}