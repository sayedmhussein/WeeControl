namespace WeeControl.Core.SharedKernel.Exceptions;

public class EntityDomainValidationException(string arg) : ArgumentOutOfRangeException(arg);