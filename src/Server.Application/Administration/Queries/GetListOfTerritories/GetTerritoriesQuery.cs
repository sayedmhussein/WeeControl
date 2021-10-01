using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Administration.DtosV1;
using WeeControl.SharedKernel.Common.DtosV1;

namespace WeeControl.Application.Administration.Queries.GetListOfTerritories
{
    public class GetTerritoriesQuery : IRequest<ResponseDto<IEnumerable<TerritoryDto>>>
    {
        public GetTerritoriesQuery(string territoryCode)
        {
            TerritoryCode = territoryCode;
        }

        public string TerritoryCode { get; private set; }
    }
}
