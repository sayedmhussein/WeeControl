using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Exceptions;

public class DomainOutOfRangeException : ArgumentOutOfRangeException
{
    public DomainOutOfRangeException(string arg) : base(arg)
    {
    }
}