using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Essential.Queries;

public class GetListOfTerritoriesQuery : IRequest<IResponseDto<IEnumerable<TerritoryDto>>>
{
    private IEnumerable<string> TerritoryCode { get; }

    public GetListOfTerritoriesQuery()
    {
        TerritoryCode = null;
    }
    
    public GetListOfTerritoriesQuery(string territoryCode)
    {
        TerritoryCode = new List<string>() { territoryCode };
    }
    
    public GetListOfTerritoriesQuery(IEnumerable<string> territoryCodes)
    {
        TerritoryCode = territoryCodes;
    }
    
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
            await userInfo.LogUserActivityAsync("Essential", "Getting List of Territories", cancellationToken);

            var list = new List<TerritoryDto>();

            if (request.TerritoryCode is null)
            {
                await context.Territories.ForEachAsync(x =>
                {
                    list.Add(new TerritoryDto()
                    {
                        TerritoryCode = x.TerritoryId, ReportToId = x.ReportToId,
                        TerritoryName = x.TerritoryName, CountryCode = x.CountryCode
                    });
                }, cancellationToken);
            }
            else
            {
                var ids = new List<string>();
                ids.AddRange(request.TerritoryCode);

                await context.Territories.Where(x => ids.Contains(x.ReportToId)).ForEachAsync(x =>
                {
                    ids.Add(x.TerritoryId);
                }, cancellationToken);
                
                var q = context.Territories.Include(a => a.Reporting).Where(x => ids.Contains(x.TerritoryId));
                
                await q.ForEachAsync(x =>
                {
                    list.Add(new TerritoryDto()
                    {
                        TerritoryCode = x.TerritoryId, ReportToId = x.ReportToId,
                        TerritoryName = x.TerritoryName, CountryCode = x.CountryCode
                    });
                }, cancellationToken);
            }

            return ResponseDto.Create<IEnumerable<TerritoryDto>>(list);
        }
    }
}