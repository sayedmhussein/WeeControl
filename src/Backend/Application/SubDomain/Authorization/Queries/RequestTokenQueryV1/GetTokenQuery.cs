using System;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.Application.SubDomain.Authorization.Queries.RequestTokenQueryV1
{
    /// <summary>
    /// A token query used to create complete response dto with employee token dto as payload.
    /// </summary>
    public class GetTokenQuery : IRequest<ResponseDto<EmployeeTokenDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Guid? SessionId { get; set; }

        public IRequestDto Request { get;  private set; }

        public GetTokenQuery(IRequestDto dto)
        {
            Request = dto;
        }
    }
}