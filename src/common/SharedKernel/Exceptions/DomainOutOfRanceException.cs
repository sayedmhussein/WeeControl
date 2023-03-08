namespace WeeControl.Core.SharedKernel.Exceptions;

public class EntityDomainValidationException : ArgumentOutOfRangeException
{
    public EntityDomainValidationException(string arg) : base(arg)
    {
    }
}