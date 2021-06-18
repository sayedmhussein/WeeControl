namespace WeeControl.SharedKernel.BasicSchemas.Common.Interfaces
{
    public interface IRequestMetadata
    {
        string Device { get; set; }

        double? Latitude { get; set; }

        double? Longitude { get; set; }
    }
}
