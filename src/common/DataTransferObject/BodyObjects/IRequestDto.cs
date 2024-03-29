﻿using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DataTransferObject.BodyObjects;

public interface IRequestDto : IEntityModel
{
    string DeviceId { get; init; }

    double? Latitude { get; init; }

    double? Longitude { get; init; }
}

public interface IRequestDto<T> : IRequestDto where T : class
{
    T Payload { get; }
}