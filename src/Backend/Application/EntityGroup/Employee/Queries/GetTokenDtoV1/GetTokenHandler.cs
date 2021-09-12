using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.SharedKernel.EntityGroup.Employee.DtosV1;

namespace WeeControl.Server.Application.Aggregates.Employee.Queries.GetTokenDtoV1
{
    public class GetTokenHandler : IRequestHandler<GetTokenQuery, EmployeeTokenDto>
    {
        public GetTokenHandler()
        {
        }

        public async Task<EmployeeTokenDto> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
             return new EmployeeTokenDto() { Token = request.Token, FullName = "User Full Name :)" };
        }
    }
}
