using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken
{
    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<EmployeeTokenDto>>
    {
        private readonly IHumanResourcesDbContext context;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;

        public GetNewTokenHandler(IHumanResourcesDbContext context, IJwtService jwtService, IMediator mediator)
        {
            this.context = context;
            this.jwtService = jwtService;
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
                
                
            }
            throw new System.NotImplementedException();
        }
    
    }
}