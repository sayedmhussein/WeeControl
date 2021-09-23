using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken
{
    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<EmployeeTokenDto>>
    {
        private readonly IHumanResourcesDbContext context;
        private readonly IJwtServiceObsolute jwtServiceObsolute;
        private readonly IMediator mediator;

        public GetNewTokenHandler(IHumanResourcesDbContext context, IJwtServiceObsolute jwtServiceObsolute, IMediator mediator)
        {
            this.context = context;
            this.jwtServiceObsolute = jwtServiceObsolute;
            this.mediator = mediator;
        }
        
        public async Task<ResponseDto<EmployeeTokenDto>> Handle(GetNewTokenQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Device))
            {
                throw new BadRequestException("Didn't provide device id.");
            }
            
            if (!string.IsNullOrWhiteSpace(request.Username) && !string.IsNullOrWhiteSpace(request.Password))
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => 
                    x.Credentials.Username == request.Username && 
                    x.Credentials.Password == request.Password, cancellationToken);

                if (employee is null) throw new NotFoundException();
                
                throw new System.NotImplementedException();
            }
            else if (request.SessionId is not null)
            {
                throw new System.NotImplementedException();
            }
            else
            {
                throw new BadRequestException("Didn't valid request query parameters.");
            }
        }
    
        private string BuildToken(IEnumerable<Claim> claims, DateTime validity)
        {
            var token = jwtServiceObsolute.GenerateJwtToken(claims, "WeeControl", validity);
            return token;
        }
    }
}