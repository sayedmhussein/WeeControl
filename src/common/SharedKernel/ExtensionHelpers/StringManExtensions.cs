namespace WeeControl.Core.SharedKernel.ExtensionHelpers;

public static class StringManExtensions
{
    public static string ToUpperFirstLetter(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return string.Empty;

        var letters = source.ToLower().ToCharArray();
        letters[0] = char.ToUpper(letters[0]);
        return new string(letters);
    }
}