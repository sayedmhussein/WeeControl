namespace WeeControl.SharedKernel.Common.Bases
{
    public abstract record ContactBase
    {
        public string ContactType { get; set; }

        public string ContactValue { get; set; }
    }
}