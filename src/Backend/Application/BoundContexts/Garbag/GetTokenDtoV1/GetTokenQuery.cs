using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;

namespace WeeControl.Backend.Application.BoundContexts.Garbag.GetTokenDtoV1
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
