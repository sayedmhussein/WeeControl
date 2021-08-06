using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;

namespace WeeControl.Server.Application.Basic.Territory.Commands.AddTerritoryV1
{
    public class AddTerritoryCommand : IRequest
    {
        public IEnumerable<TerritoryDto> TerritoryDtos { get; set; }
    }
}
