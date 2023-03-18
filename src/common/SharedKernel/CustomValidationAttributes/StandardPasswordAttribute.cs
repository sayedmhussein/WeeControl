using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.CustomValidationAttributes;

public class StandardPasswordAttribute : ValidationAttribute
{
    private const string SpecialLetters = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
    private const string Numbers = "1234567890";
    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    
    public override bool IsValid(object? value)
    {
        var str = value as string;
        if (string.IsNullOrWhiteSpace(str))
            return false;

        if (str.Length < 6)
            return false;

        foreach (var s in str)
        {
            if (SpecialLetters.Contains(s))
            {
                foreach (var n in str)
                {
                    if (Numbers.Contains(n))
                    {
                        foreach (var c in str)
                        {
                            if (Letters.ToUpper().Contains(c))
                            {
                                foreach (var sm in str)
                                {
                                    if (Letters.ToLower().Contains(sm))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return "Password should contains at least one capital letter, one small letter, one number, one special letter and size should be more than 6 letters.";
    }
}