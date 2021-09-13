using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.SharedKernel.DtosV1.Employee;

namespace WeeControl.Backend.Application.EntityGroups.Employee.Queries.GetTokenDtoV1
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
