using System;
using System.Collections.Immutable;
using MySystem.User.Employee.Enumerators;

namespace MySystem.User.Employee.Interfaces
{
    public interface IEmployeeLibValues
    {
        ImmutableDictionary<ApplicationPageEnum, string> ApplicationPage { get; }
    }
}
