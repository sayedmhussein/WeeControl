using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WeeControl.Backend.Application.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Failures { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Failures = new Dictionary<string, string[]>();
    }

    public ValidationException(ICollection<ValidationResult> failures)
        : this()
    {
            
        var propertyNames = failures
            .Select(e => e.MemberNames)
            .Distinct();

        foreach (var propertyName in propertyNames)
        {
            var propertyFailures = failures
                .Where(e => e.MemberNames == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            Failures.Add(propertyName.FirstOrDefault(), propertyFailures);
        }
    }
}