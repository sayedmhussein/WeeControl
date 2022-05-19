using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Queries;

public class GetListOfTerritoriesQuery : IRequest<IResponseDto<IEnumerable<TerritoryDto>>>
{
    
    public class GetListOfTerritoriesHandler : IRequestHandler<GetListOfTerritoriesQuery, IResponseDto<IEnumerable<TerritoryDto>>>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo userInfo;

        public GetListOfTerritoriesHandler(IEssentialDbContext context, ICurrentUserInfo userInfo)
        {
            this.context = context;
            this.userInfo = userInfo;
        }
        
        public async Task<IResponseDto<IEnumerable<TerritoryDto>>> Handle(GetListOfTerritoriesQuery request, CancellationToken cancellationToken)
        {
            await userInfo.LogUserActivityAsync("", "Get Terriroes", cancellationToken);

            var list = new List<TerritoryDto>();
            await context.Territories.ForEachAsync(x =>
            {
                list.Add(new TerritoryDto()
                {
                    TerritoryCode = x.TerritoryCode, ReportToId = x.ReportToId,
                    TerritoryName = x.TerritoryName, CountryCode = x.CountryCode
                });
            }, cancellationToken);

            return new ResponseDto<IEnumerable<TerritoryDto>>(list);
        }
    }
}