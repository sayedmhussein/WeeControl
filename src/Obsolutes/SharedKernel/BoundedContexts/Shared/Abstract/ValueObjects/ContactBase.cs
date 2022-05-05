namespace WeeControl.Common.SharedKernel.BoundedContexts.Shared.Abstract.ValueObjects
{
    public abstract record ContactBase
    {
        public string ContactType { get; set; }

        public string ContactValue { get; set; }
    }
}