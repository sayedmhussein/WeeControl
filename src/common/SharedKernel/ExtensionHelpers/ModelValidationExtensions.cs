using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.ExtensionHelpers;

public static class ModelValidationExtensions
{
    public static IDictionary<string, string?[]> GetModelValidationErrors(this IValidatableModel validatable)
    {
        return GetErrors(validatable);
    }

    public static string GetFirstValidationError(this IValidatableModel validatable)
    {
        var errors = GetErrors(validatable);
        if (!errors.Any()) return string.Empty;

        var pair = errors.First();
        return pair.Key + " - " + pair.Value.FirstOrDefault();
    }

    public static bool IsValidEntityModel(this IValidatableModel validatable)
    {
        return !GetModelValidationErrors(validatable).Any();
    }

    public static void ThrowExceptionIfEntityModelNotValid(this IValidatableModel validatable)
    {
        var errors = GetModelValidationErrors(validatable);
        if (errors.Any())
            throw new EntityModelValidationException(errors);
    }

    private static IDictionary<string, string?[]> GetErrors(object entity)
    {
        var list = new Dictionary<string, string?[]>();

        var validationResults = new List<ValidationResult>();

        var result = Validator.TryValidateObject(
            entity,
            new ValidationContext(entity),
            validationResults,
            true);

        if (result) return list;

        var propertyNames = validationResults
            .Select(e => e.MemberNames)
            .Distinct();

        foreach (var propertyName in propertyNames)
        {
            var propertyFailures = validationResults
                .Where(e => e.MemberNames == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            list.Add(propertyName.FirstOrDefault() ?? string.Empty, propertyFailures);
        }

        return list;
    }
}