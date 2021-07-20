using MediatR;
using WeeControl.SharedKernel.BasicSchemas.Territory.Entities.DtosV1;

namespace WeeControl.Server.Application.Basic.Territory.V1.Commands
{
    public class AddTerritoryCommand : IRequest
    {
        public TerritoryDto TerritoryDto { get; set; }
    }
}
