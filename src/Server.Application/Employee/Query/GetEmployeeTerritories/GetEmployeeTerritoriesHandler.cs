using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;

namespace MySystem.Application.Employee.Query.GetEmployeeTerritories
{
    public class GetEmployeeTerritoriesHandler : IRequestHandler<GetEmployeeTerritoriesQuery, IEnumerable<Guid>>
    {
        private readonly IMySystemDbContext context;

        public GetEmployeeTerritoriesHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Guid>> Handle(GetEmployeeTerritoriesQuery request, CancellationToken cancellationToken)
        {
            var employee = await context.EmployeeSessions.FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);
            if (employee == null)
            {
                throw new NotFoundException("Session Not Found!", "");
            }

            var ids = new List<Guid>();
            var ids_ = await context.Territories.Where(x => x.ReportToId == employee.Employee.TerritoryId).ToListAsync();
            ids_.ForEach(x => ids.Add(x.Id));

            return ids;
        }
    }
}
