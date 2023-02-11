using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WeeControl.Core.Domain.Exceptions;

public class DomainValidationException : DomainOutOfRangeException
{
    public static void ValidateEntity(object entity)
    {
        var validationResults = new List<ValidationResult>();
        
        var result = Validator.TryValidateObject(
            entity,
            new ValidationContext(entity),
            validationResults,
            true);
        
        if (result == false)
        {
            throw new DomainValidationException(validationResults);
        }
    }
    public IDictionary<string, string[]> Failures { get; } = new Dictionary<string, string[]>();

    public DomainValidationException(ICollection<ValidationResult> failures)
        : base($"Check property named \"Failures\" for more details. Member(s) { failures.FirstOrDefault()?.MemberNames.FirstOrDefault() } has error {failures.FirstOrDefault()?.ErrorMessage}")
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

            Failures.Add(propertyName.FirstOrDefault() ?? string.Empty, propertyFailures);
        }
    }
}