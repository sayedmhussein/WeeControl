using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.SharedKernel.Entities.Territory.V1Dto;

namespace MySystem.Application.Territory.Query.GetTerritories
{
    public class GetTerritoriesV1Handler : IRequestHandler<GetTerritoriesV1Query, IEnumerable<TerritoryDto>>
    {
        private readonly IMySystemDbContext context;

        public GetTerritoriesV1Handler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TerritoryDto>> Handle(GetTerritoriesV1Query request, CancellationToken cancellationToken)
        {
            EmployeeDbo employee = null;

            if (request.EmployeeId != null)
            {
                employee = await context.Employees.FirstOrDefaultAsync(s => s.Id == request.EmployeeId, cancellationToken);
            }
            else if (request.SessionId != null)
            {
                employee = (await context.EmployeeSessions.FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken))?.Employee;
            }

            if (employee == null)
            {
                throw new NotFoundException("Employee Not Found!", "");
            }

            var ids = new List<TerritoryDto>();

            List<Guid> childrens = new List<Guid>() { employee.TerritoryId };
            //await context.Territories.ForEachAsync(x => childrens.Contains(x) ? childrens.Add(x) : _);

            //var office = await context.Territories.Include(x => x.ReportToId).ToListAsync();
            //var ids_ = context.Territories.Where(x => )
            var par = context.Territories.FirstOrDefault(x => x.Id == employee.TerritoryId);
            var chd = context.Territories.Where(x => x.Id == employee.TerritoryId).SelectMany(x => x.ReportingFrom).ToList();
            chd.Add(par);
            //var ids_ = context.Territories.Where(x => x.Id == employee.TerritoryId).Union(context.Territories.Where(x => x.Id == employee.TerritoryId).Include(x => x.ReportingFrom).ToList();
            //var ids_ = await context.Territories.Where(x => x.ReportToId == employee.TerritoryId).ToListAsync();
            chd.ForEach(x => ids.Add(new TerritoryDto() { Id = x.Id, Name = x.Name }));

            return ids;
        }
    }
}
