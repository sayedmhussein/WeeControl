using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.ExtensionMethods;

public static class ModelValidationExtensions
{
    public static IDictionary<string, string?[]> GetModelValidationErrors(this IEntityModel entity)
    {
        return GetErrors(entity);
    }

    public static string GetFirstValidationError(this IEntityModel entity)
    {
        var errors = GetErrors(entity);
        if (!errors.Any()) return string.Empty;

        var pair = errors.First();
        return pair.Key + " - " + pair.Value.FirstOrDefault();
    }

    public static bool IsValidEntityModel(this IEntityModel entity)
    {
        return !GetModelValidationErrors(entity).Any();
    }

    public static void ThrowExceptionIfEntityModelNotValid(this IEntityModel entity)
    {
        var errors = GetModelValidationErrors(entity);
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