using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;

namespace WeeControl.Server.Application.Territory.V1.Commands
{
    public class AddOrEditTerritoriesCommand : IRequest<IEnumerable<TerritoryDto>>
    {
        public ICollection<TerritoryDto> TerritoryDtos { get; set; }
    }
}
