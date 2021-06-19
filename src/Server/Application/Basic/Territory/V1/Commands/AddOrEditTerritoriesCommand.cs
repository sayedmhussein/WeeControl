using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;

namespace WeeControl.Server.Application.Territory.V1.Commands
{
    public class UpdateTerritoryCommand : IRequest<Unit>
    {
        public TerritoryDto TerritoryDto { get; set; }
    }
}
