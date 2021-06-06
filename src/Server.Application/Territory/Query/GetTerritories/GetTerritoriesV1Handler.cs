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
using MySystem.SharedKernel.EntityV1Dtos.Territory;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Application.Territory.Query.GetTerritories
{
    public class GetTerritoriesV1Handler : IRequestHandler<GetTerritoriesV1Query, IEnumerable<TerritoryDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo userInfo;
        private readonly ISharedValues values;

        public GetTerritoriesV1Handler(IMySystemDbContext context, ICurrentUserInfo userInfo, ISharedValues values)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
            this.values = values ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<TerritoryDto>> Handle(GetTerritoriesV1Query request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Query can't be null!");
            }

            if (request.EmployeeId == null && request.SessionId == null && request.TerritoryId == null)
            {
                return GetListOfTerritores(userInfo.TerritoriesId);
            }

            var tag = userInfo.Claims.FirstOrDefault(x=>x.Type == values.ClaimType[ClaimTypeEnum.HumanResources])?.Value?.Contains(values.ClaimTag[ClaimTagEnum.Read]);
            if (tag == false || tag == null)
            {
                throw new NotAllowedException("");
            }

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

            //List<Guid> childrens = new List<Guid>() { employee.TerritoryId };

            var par = context.Territories.FirstOrDefault(x => x.Id == employee.TerritoryId);
            var chd = context.Territories.Where(x => x.Id == employee.TerritoryId).SelectMany(x => x.ReportingFrom).ToList();
            chd.Add(par);
            
            chd.ForEach(x => ids.Add(new TerritoryDto() { Id = x.Id, Name = x.Name }));

            return ids;
        }

        private List<TerritoryDto> GetListOfTerritores(IEnumerable<Guid> listOfTerritores)
        {
            var ids = new List<TerritoryDto>();

            var par = context.Territories.FirstOrDefault(x => listOfTerritores.Contains(x.Id));
            var chd = context.Territories.Where(x => listOfTerritores.Contains(x.Id)).SelectMany(x => x.ReportingFrom).ToList();
            chd.Add(par);

            chd.ForEach(x => ids.Add(new TerritoryDto() { Id = x.Id, Name = x.Name }));

            return ids;
        }
    }
}
