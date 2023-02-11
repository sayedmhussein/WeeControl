using System;

namespace WeeControl.Core.Domain.Exceptions;

public class DomainOutOfRangeException : ArgumentOutOfRangeException
{
    public DomainOutOfRangeException(string arg) : base(arg)
    {
    }
}