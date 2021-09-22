namespace WeeControl.Common.SharedKernel.Abstract.ValueObjects
{
    public abstract record Coordinates
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}