using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DataTransferObject.BodyObjects;

public interface IRequestDto : IEntityModel
{
    string DeviceId { get; set; }

    double? Latitude { get; set; }

    double? Longitude { get; set; }
}

public interface IRequestDto<T> : IRequestDto where T : class
{
    T Payload { get; }
}