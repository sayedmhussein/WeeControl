using MediatR;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.AddTerritoryV1
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
