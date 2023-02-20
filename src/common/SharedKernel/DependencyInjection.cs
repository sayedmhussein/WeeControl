using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

namespace WeeControl.Core.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }

    public static IDictionary<string, string[]> GetModelValidationError(this IEntityModel entity)
    {
        var list = new Dictionary<string, string[]>();
        
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