using System;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;

namespace WeeControl.Server.Application.Aggregates.Employee.Queries.GetTokenDtoV1
{
    public class GetTokenQuery : IRequest<EmployeeTokenDto>
    {
        public string Token { get; set; }

        public GetTokenQuery(string token)
        {
            Token = token;
        }
    }
}
