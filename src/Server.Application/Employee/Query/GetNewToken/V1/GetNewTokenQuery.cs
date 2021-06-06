using MediatR;
using MySystem.SharedKernel.Interfaces;

namespace Application.Employee.Query.GetNewToken.V1
{
    public class GetNewTokenQuery : IRequest<IResponseDto<string>>
    {
    }
}
