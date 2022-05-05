using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetListOfTerritories
{
    public class GetTerritoriesHandler : IRequestHandler<GetTerritoriesQuery, ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>
    {
        private readonly IHumanResourcesDbContext context;
        private readonly ICurrentUserInfo userInfo;

        public GetTerritoriesHandler(IHumanResourcesDbContext context, ICurrentUserInfo userInfo)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
        }

        public Task<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>> Handle(GetTerritoriesQuery request, CancellationToken cancellationToken)
        {
            // if (request != null)
            // {
            //     return GetListOfTerritoresByTerritoryIdAsync(request.TerritoryId, cancellationToken);
            // }
            // else
            // {
            //     throw new ArgumentNullException("Query can't be null!");
            // }
            throw new NotImplementedException();
        }

        // private async Task<IEnumerable<IdentifiedTerritoryDto>> GetListOfTerritores(CancellationToken cancellationToken)
        // {
        //     // var list = new List<IdentifiedTerritoryDto>();
        //     // var territories = await context.Territories.ToListAsync(cancellationToken);
        //     //
        //     // territories.ForEach(x => list.Add(x.ToDto<Territory, IdentifiedTerritoryDto>()));
        //     // return list;
        // }

        // private async Task<IEnumerable<IdentifiedTerritoryDto>> GetListOfTerritoresByTerritoryIdAsync(Guid? territoryid, CancellationToken cancellationToken)
        // {
        //     // var list = new List<IdentifiedTerritoryDto>();
        //     // var territories = await context.Territories.ToListAsync(cancellationToken);
        //     //
        //     // if (territoryid == null)
        //     // {
        //     //     territories.ForEach(x => list.Add(x.ToDto<TerritoryDbo, IdentifiedTerritoryDto>()));
        //     //     return list;
        //     // }
        //     // else
        //     // {
        //     //     var par = territories.FirstOrDefault(x => x.Id == (Guid)territoryid);
        //     //     var chd = GetChildrenFromList((Guid)territoryid, territories);
        //     //     chd.Add(par);
        //     //
        //     //     chd.ForEach(x => list.Add(x.ToDto<Territory, IdentifiedTerritoryDto>()));
        //     //
        //     //     if (list.FirstOrDefault() == null)
        //     //     {
        //     //         throw new NotFoundException();
        //     //     }
        //     //
        //     //     return list;
        //     // }
        // }

        // private List<TerritoryDbo> GetChildrenFromList(Guid parent, List<TerritoryDbo> dbos)
        // {
        //     // var list = new List<TerritoryDbo>();
        //     //
        //     // var children = dbos.Where(x => x.ReportToId == parent).ToList();
        //     // if (children.Count == 0)
        //     // {
        //     //     return list;
        //     // }
        //     // else
        //     // {
        //     //     list.AddRange(children);
        //     //     foreach (var child in children)
        //     //     {
        //     //         dbos.Remove(child);
        //     //         list.AddRange(GetChildrenFromList(child.Id, dbos));
        //     //     }
        //     //     
        //     //     return list;
        //     // }
        // }
    }
}
