using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.SharedKernel.Aggregates.Territory;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;
using WeeControl.SharedKernel.Extensions;

namespace WeeControl.Server.Application.Territory.V1.Queries
{
    public class GetTerritoriesHandler : IRequestHandler<GetTerritoriesQuery, IEnumerable<TerritoryDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo userInfo;
        private readonly ITerritoryLists territoryLists;

        public GetTerritoriesHandler(IMySystemDbContext context, ICurrentUserInfo userInfo, ITerritoryLists territoryLists)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
            this.territoryLists = territoryLists ?? throw new ArgumentNullException();
        }

        public Task<IEnumerable<TerritoryDto>> Handle(GetTerritoriesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Query can't be null!");
            }

            if (request.TerritoryId != null && request.SessionId == null && request.EmployeeId == null)
            {
                return GetListOfTerritoresByTerritoryIdAsync((Guid)request.TerritoryId, cancellationToken);
            }
            else if (request.TerritoryId == null && request.SessionId != null && request.EmployeeId == null)
            {
                return GetListOfTerritoriesBySessionIdAsync((Guid)request.SessionId, cancellationToken);
            }
            else if (request.TerritoryId == null && request.SessionId == null && request.EmployeeId != null)
            {
                return GetListOfTerritoriesByEmployeeIdAsync((Guid)request.EmployeeId, cancellationToken);
            }
            else
            {
                return GetListOfTerritores(cancellationToken);
            }
        }

        private async Task<IEnumerable<TerritoryDto>> GetListOfTerritores(CancellationToken cancellationToken)
        {
            var list = new List<TerritoryDto>();
            var territories = await context.Territories.ToListAsync(cancellationToken);

            territories.ForEach(x => list.Add(x.ToDto<TerritoryDbo, TerritoryDto>()));
            return list;
        }

        private async Task<IEnumerable<TerritoryDto>> GetListOfTerritoresByTerritoryIdAsync(Guid? territoryid, CancellationToken cancellationToken)
        {
            var list = new List<TerritoryDto>();
            var territories = await context.Territories.ToListAsync(cancellationToken);

            if (territoryid == null)
            {
                territories.ForEach(x => list.Add(x.ToDto<TerritoryDbo, TerritoryDto>()));
                return list;
            }
            else
            {
                var par = territories.FirstOrDefault(x => x.Id == (Guid)territoryid);
                var chd = GetChildrenFromList((Guid)territoryid, territories);
                chd.Add(par);

                chd.ForEach(x => list.Add(x.ToDto<TerritoryDbo, TerritoryDto>()));

                if (list.FirstOrDefault() == null)
                {
                    throw new NotFoundException();
                }

                return list;
            }
        }

        private async Task<IEnumerable<TerritoryDto>> GetListOfTerritoriesBySessionIdAsync(Guid sessiondid, CancellationToken cancellationToken)
        {
            var session = await context.EmployeeSessions.FirstOrDefaultAsync(s => s.Id == sessiondid && s.TerminationTs == null);
            if (session == null)
            {
                throw new NotFoundException("", "");
            }

            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == session.EmployeeId);

            return await GetListOfTerritoriesByEmployeeIdAsync(employee.Id, cancellationToken);
        }

        private async Task<IEnumerable<TerritoryDto>> GetListOfTerritoriesByEmployeeIdAsync(Guid employeeid, CancellationToken cancellationToken)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == employeeid, cancellationToken);
            if (employee == null)
            {
                throw new NotFoundException("Employee Not Found!", "");
            }

            return await GetListOfTerritoresByTerritoryIdAsync(employee.TerritoryId, cancellationToken);
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
