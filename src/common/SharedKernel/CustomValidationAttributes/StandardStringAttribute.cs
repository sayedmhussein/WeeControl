using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeeControl.Core.SharedKernel.CustomValidationAttributes;

public class StandardStringAttribute : ValidationAttribute
{
    private const string DefaultString = "abcdefghijklmnopqrstuvwxyz1234567890_";
    private string? accept;
    private string? reject;

    public StandardStringAttribute(string? accept = null, string? reject = null)
    {
        this.accept = accept?.ToLower();
        this.reject = reject?.ToLower();
    }

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

    public override bool IsValid(object? value)
    {
        var str = value as string;

        if (string.IsNullOrEmpty(str) == false)
        {
            var allowed = DefaultString;

            if (string.IsNullOrEmpty(accept) == false) allowed += accept;

            foreach (var c in str.ToLower())
            {
                if (allowed.Contains(c) == false) return false;

                if (string.IsNullOrEmpty(reject) == false)
                    if (reject.Contains(c))
                        return false;
            }
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        var sp = new StringBuilder();
        sp.Append("Text should only contains letters, numbers");

        if (string.IsNullOrEmpty(accept))
        {
            sp.Append(" and \"_\"");
        }
        else
        {
            sp.Append(", \"_\" and any of \"");
            sp.Append(accept);
            sp.Append("\"");
        }

        if (string.IsNullOrEmpty(reject))
        {
        }
        else
        {
            sp.Append(", and not include \"");
            sp.Append(reject);
            sp.Append('"');
        }

        sp.Append('.');

        return sp.ToString();
    }
}