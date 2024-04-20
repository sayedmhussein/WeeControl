using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.DtoParent;

public interface IRequestDto : IValidatableModel
{
    string DeviceId { get; init; }

    double? Latitude { get; init; }

    double? Longitude { get; init; }
}

public interface IRequestDto<T> : IRequestDto where T : class
{
    T Payload { get; }
}