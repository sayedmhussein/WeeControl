using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Queries
{
    public class GetTokenHandler : IRequestHandler<GetTokenQuery, ResponseDto<LoginDto>>
    {
        private readonly ICredentialsDbContext context;

        public GetTokenHandler(ICredentialsDbContext context, IJwtService jwtService, IMediator mediator, IConfiguration configuration)
        {
            this.context = context;
        }

        public Task<ResponseDto<LoginDto>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
