using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Application.Territory.V1.Commands;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.Server.Domain.Extensions;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;

namespace WeeControl.Server.Application.Territory.V1.Handlers
{
    public class UpdateTerritoryHandler : IRequestHandler<UpdateTerritoryCommand, Unit>
    {
        private readonly IMySystemDbContext context;

        public UpdateTerritoryHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateTerritoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.TerritoryDto == null)
            {
                throw new ArgumentNullException();
            }

            //var dbo = request.TerritoryDto.ToDbo<TerritoryDto, TerritoryDbo>();

            var item = await context.Territories.FirstOrDefaultAsync(x => x.Id == request.TerritoryDto.Id);
            if (item == null)
            {
                throw new NotFoundException();
            }

            

            try
            {
                item.CountryId = request.TerritoryDto.CountryId;
                item.Name = request.TerritoryDto.Name;
                item.ReportToId = request.TerritoryDto.ReportToId;

                context.Territories.Update(item);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }

            

            return Unit.Value;
        }
    }
}
