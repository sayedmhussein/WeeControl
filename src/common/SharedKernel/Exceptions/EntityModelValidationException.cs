namespace WeeControl.Core.SharedKernel.Exceptions;

public class EntityModelValidationException : ArgumentException
{
    public EntityModelValidationException(IDictionary<string, string?[]> errors) : base(
        "Entity model has invalid data, check failures property.")
    {
        Failures = errors;
    }

    public IDictionary<string, string?[]> Failures { get; }
}