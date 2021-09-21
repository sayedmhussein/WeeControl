using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Domain.EntityGroups.Territory;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Territory;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;
using WeeControl.Common.SharedKernel.Extensions;

namespace WeeControl.Backend.Application.BoundContexts.Adminstration.Territory.Queries.GetTerritoryV1
{
    public class GetTerritoriesHandler : IRequestHandler<GetTerritoriesQuery, IEnumerable<IdentifiedTerritoryDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo userInfo;
        private readonly ITerritoryAttribute territoryAttribute;

        public GetTerritoriesHandler(IMySystemDbContext context, ICurrentUserInfo userInfo, ITerritoryAttribute territoryAttribute)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
            this.territoryAttribute = territoryAttribute ?? throw new ArgumentNullException();
        }

        public Task<IEnumerable<IdentifiedTerritoryDto>> Handle(GetTerritoriesQuery request, CancellationToken cancellationToken)
        {
            if (request != null)
            {
                return GetListOfTerritoresByTerritoryIdAsync(request.TerritoryId, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException("Query can't be null!");
            }
        }

        private async Task<IEnumerable<IdentifiedTerritoryDto>> GetListOfTerritores(CancellationToken cancellationToken)
        {
            var list = new List<IdentifiedTerritoryDto>();
            var territories = await context.Territories.ToListAsync(cancellationToken);

            territories.ForEach(x => list.Add(x.ToDto<TerritoryDbo, IdentifiedTerritoryDto>()));
            return list;
        }

        private async Task<IEnumerable<IdentifiedTerritoryDto>> GetListOfTerritoresByTerritoryIdAsync(Guid? territoryid, CancellationToken cancellationToken)
        {
            var list = new List<IdentifiedTerritoryDto>();
            var territories = await context.Territories.ToListAsync(cancellationToken);

            if (territoryid == null)
            {
                territories.ForEach(x => list.Add(x.ToDto<TerritoryDbo, IdentifiedTerritoryDto>()));
                return list;
            }
            else
            {
                var par = territories.FirstOrDefault(x => x.Id == (Guid)territoryid);
                var chd = GetChildrenFromList((Guid)territoryid, territories);
                chd.Add(par);

                chd.ForEach(x => list.Add(x.ToDto<TerritoryDbo, IdentifiedTerritoryDto>()));

                if (list.FirstOrDefault() == null)
                {
                    throw new NotFoundException();
                }

                return list;
            }
        }

        private List<TerritoryDbo> GetChildrenFromList(Guid parent, List<TerritoryDbo> dbos)
        {
            var list = new List<TerritoryDbo>();

            var children = dbos.Where(x => x.ReportToId == parent).ToList();
            if (children.Count == 0)
            {
                return list;
            }
            else
            {
                list.AddRange(children);
                foreach (var child in children)
                {
                    dbos.Remove(child);
                    list.AddRange(GetChildrenFromList(child.Id, dbos));
                }
                
                return list;
            }
        }
    }
}
