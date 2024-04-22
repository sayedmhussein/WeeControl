namespace WeeControl.Core.SharedKernel.Exceptions;

public class EntityModelValidationException(IDictionary<string, string?[]> errors)
    : ArgumentException("Entity model has invalid data, check failures property.")
{
    public IDictionary<string, string?[]> Failures { get; } = errors;
}