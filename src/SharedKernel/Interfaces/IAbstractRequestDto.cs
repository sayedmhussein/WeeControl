using System;
namespace WeeControl.SharedKernel.Interfaces
{
    public interface IAbstractRequestDto
    {
        /// <summary>
        /// A Unique String of the Client Device.
        /// </summary>
        string DeviceId { get; set; }

        double? Latitude { get; set; }

        double? Longitude { get; set; }
    }
}
