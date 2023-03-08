namespace WeeControl.Core.SharedKernel.Exceptions;

public class EntityModelValidationException : ArgumentException
{
    public IDictionary<string, string?[]> Failures { get; }
    
    public EntityModelValidationException(IDictionary<string, string?[]> errors) : base("Entity model has invalid data, check failures property.")
    {
        Failures = errors;
    }
}