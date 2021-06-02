using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.Entities.Public.V1Dto;

namespace Application.Employee.Command.GetRefreshedToken.V1
{
    public class GetRefreshedTokenQuery : IRequest<ResponseDto<string>>
    {
    }
}
