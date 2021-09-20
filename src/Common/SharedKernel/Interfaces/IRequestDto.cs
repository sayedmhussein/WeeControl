namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IRequestDto
    {
        /// <summary>
        /// A Unique String of the Client Device.
        /// </summary>
        string DeviceId { get; set; }

        double? Latitude { get; set; }

        double? Longitude { get; set; }
    }
}
