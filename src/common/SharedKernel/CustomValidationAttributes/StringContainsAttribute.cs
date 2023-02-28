using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.CustomValidationAttributes;

public class StringContainsAttribute : ValidationAttribute
{
    private const string DefaultString = "abcdefghijklmnopqrstuvwxyz1234567890_";
    private string? accept;
    private string? reject;
    
    public string? Accept
    {
        get => accept;
        set => accept = value?.ToLower();
    }

    public string? Reject
    {
        get => reject;
        set => reject = value?.ToLower();
    }

    public StringContainsAttribute(string? accept = null, string? reject = null)
    {
        this.accept = accept?.ToLower();
        this.reject = reject?.ToLower();
    }

    public override bool IsValid(object? value)
    {
        var str = value as string;
        
        if (string.IsNullOrEmpty(str) == false)
        {
            var allowed = DefaultString;

            if (string.IsNullOrEmpty(accept) == false)
            {
                allowed += accept;
            }

            foreach (var c in str.ToLower())
            {
                if (allowed.Contains(c) == false)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(reject) == false)
                {
                    if (reject.Contains(c))
                    {
                        return false;
                    }
                }
            }
        }
        
        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return "Must Contains.";
    }
}