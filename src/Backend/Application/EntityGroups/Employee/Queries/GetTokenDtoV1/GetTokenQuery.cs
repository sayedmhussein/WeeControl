using MediatR;
using WeeControl.SharedKernel.DtosV1.Employee;

namespace WeeControl.Backend.Application.EntityGroups.Employee.Queries.GetTokenDtoV1
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
