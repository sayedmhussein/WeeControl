using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Contexts.Essential.Queries;

public class TerritoryQuery : IRequest<IResponseDto<IEnumerable<TerritoryDto>>>
{
    private readonly ICollection<string> territoryCodes;

    public TerritoryQuery()
    {
        territoryCodes = null;
    }
    
    public TerritoryQuery(string territoryCode)
    {
        this.territoryCodes = new List<string>() { territoryCode };
    }
    
    public TerritoryQuery(ICollection<string> territoryCodes)
    {
        this.territoryCodes = territoryCodes;
    }
    
    public class GetListOfTerritoriesHandler : IRequestHandler<TerritoryQuery, IResponseDto<IEnumerable<TerritoryDto>>>
    {
        private readonly IEssentialDbContext context;

        public GetListOfTerritoriesHandler(IEssentialDbContext context)
        {
            this.context = context;
        }
        
        public async Task<IResponseDto<IEnumerable<TerritoryDto>>> Handle(TerritoryQuery request, CancellationToken cancellationToken)
        {
            var list = new List<TerritoryDto>();

            if (request.territoryCodes is null)
            {
                await context.Territories.ForEachAsync(x =>
                {
                    list.Add(new TerritoryDto()
                    {
                        LocalName = x.AlternativeName,
                        TerritoryCode = x.TerritoryIdObsolute, ReportToId = x.ReportToIdObsolute,
                        TerritoryName = x.UniqueName, CountryCode = x.CountryCode
                    });
                }, cancellationToken);
            }
            else
            {
                var ids = new List<string>();
                ids.AddRange(request.territoryCodes);

                await context.Territories.Where(x => ids.Contains(x.ReportToIdObsolute)).ForEachAsync(x =>
                {
                    ids.Add(x.TerritoryIdObsolute);
                }, cancellationToken);
                
                var q = context.Territories.Include(a => a.Reporting).Where(x => ids.Contains(x.TerritoryIdObsolute));
                
                await q.ForEachAsync(x =>
                {
                    list.Add(new TerritoryDto()
                    {
                        LocalName = x.AlternativeName,
                        TerritoryCode = x.TerritoryIdObsolute, ReportToId = x.ReportToIdObsolute,
                        TerritoryName = x.UniqueName, CountryCode = x.CountryCode
                    });
                }, cancellationToken);
            }

            return ResponseDto.Create<IEnumerable<TerritoryDto>>(list);
        }
    }
}