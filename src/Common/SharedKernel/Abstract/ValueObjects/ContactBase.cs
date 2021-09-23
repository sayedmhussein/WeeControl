namespace WeeControl.Common.SharedKernel.Abstract.ValueObjects
{
    public abstract record ContactBase
    {
        public string ContactType { get; set; }

        public string ContactValue { get; set; }
    }
}