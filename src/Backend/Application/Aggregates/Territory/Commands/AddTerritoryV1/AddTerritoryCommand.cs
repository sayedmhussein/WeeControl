using System;
using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.Application.Basic.Territory.Commands.AddTerritoryV1
{
    public class AddTerritoryCommand : IRequest<IdentifiedTerritoryDto>, IRequestDto<TerritoryDto>
    {
        public TerritoryDto Payload { get; set; }

        public string DeviceId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public AddTerritoryCommand(IRequestDto<TerritoryDto> dto)
        {
            Payload = dto.Payload;

            DeviceId = dto.DeviceId;
            Latitude = dto.Latitude;
            Longitude = dto.Longitude;
        }
    }
}
