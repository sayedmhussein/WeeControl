// using MediatR;
// using WeeControl.SharedKernel.Adminstration.DtosV1;
// using WeeControl.SharedKernel.Common.Interfaces;
//
// namespace WeeControl.Application.HumanResources.Commands.AddTerritoryV1
// {
//     public class AddTerritoryCommand : IRequest<TerritoryDto>, IRequestDto<Dto>
//     {
//         public Dto Payload { get; set; }
//
//         public string DeviceId { get; set; }
//         public double? Latitude { get; set; }
//         public double? Longitude { get; set; }
//
//         public AddTerritoryCommand(IRequestDto<Dto> dto)
//         {
//             Payload = dto.Payload;
//
//             DeviceId = dto.DeviceId;
//             Latitude = dto.Latitude;
//             Longitude = dto.Longitude;
//         }
//     }
// }
