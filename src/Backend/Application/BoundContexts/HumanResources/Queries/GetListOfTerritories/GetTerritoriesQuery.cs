using System;
using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Territory;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetListOfTerritories
{
    public class GetTerritoriesQuery : IRequest<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>
    {
        public GetTerritoriesQuery(string territoryCode)
        {
            TerritoryCode = territoryCode;
        }

        public string TerritoryCode { get; private set; }
    }
}
