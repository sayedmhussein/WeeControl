using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.CustomValidationAttributes;

public class StringContainsAttribute : ValidationAttribute
{
    private const string allowedCharacters = "abcdefghijklmnopqrstuvwxyz1234567890_";
    
    public string? Accept { get; set; }
    public string? Reject { get; set; }

    public override bool IsValid(object? value)
    {
        var str = (string)value;
        
        if (string.IsNullOrEmpty(str))
        {
            return true;
        }

        foreach (var c in str.ToLower())
        {
            if (allowedCharacters.Contains(c) == false)
            {
                return false;
            }
        }
        
        return base.IsValid(value);
    }

    public override string FormatErrorMessage(string name)
    {
        return "Must Contains.";
    }
}