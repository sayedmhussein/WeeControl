using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo.Territory;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.EntityV1Dtos.Territory;
using MySystem.SharedKernel.Interfaces.Values;

namespace MySystem.Application.Territory.Query.GetTerritories
{
    public class GetTerritoriesV1Handler : IRequestHandler<GetTerritoriesV1Query, IEnumerable<TerritoryDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo userInfo;
        private readonly ITerritoryValues values;

        public GetTerritoriesV1Handler(IMySystemDbContext context, ICurrentUserInfo userInfo, ITerritoryValues values)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
            this.values = values ?? throw new ArgumentNullException();
        }

        public Task<IEnumerable<TerritoryDto>> Handle(GetTerritoriesV1Query request, CancellationToken cancellationToken)
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
                throw new BadRequestException("Didn't provide correct query!");
            }
        }

        private async Task<IEnumerable<TerritoryDto>> GetListOfTerritoresByTerritoryIdAsync(Guid territoryid, CancellationToken cancellationToken)
        {
            var ids = new List<TerritoryDto>();

            var allTerritories = await context.Territories.ToListAsync(cancellationToken);

            var par = allTerritories.FirstOrDefault(x => x.Id == territoryid);
            var chd = GetChildrenFromList(territoryid, allTerritories);
            chd.Add(par);

            //var par = context.Territories.FirstOrDefault(x => x.Id == territoryid);
            //var chd = context.Territories.Where(x => x.Id == territoryid).SelectMany(x => x.ReportingFrom).ToList();
            //chd.Add(par);

            chd.ForEach(x => ids.Add(x.ToDto<TerritoryDbo, TerritoryDto>()));

            return ids;
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
