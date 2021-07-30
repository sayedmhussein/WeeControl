namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IRequestMetadata
    {
        string Device { get; set; }

        double? Latitude { get; set; }

        double? Longitude { get; set; }
    }
}
