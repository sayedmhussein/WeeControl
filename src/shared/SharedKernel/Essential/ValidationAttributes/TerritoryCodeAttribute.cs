using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | 
                AttributeTargets.Field, AllowMultiple = false)]
public sealed class TerritoryCodeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new ValidationResult("Territory code can't be null");
        
        var code = (string)value;

        if (code.Length <= 10){}

        return null;
    }
}