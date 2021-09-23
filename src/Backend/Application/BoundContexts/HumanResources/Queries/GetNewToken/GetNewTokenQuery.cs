using System;
using MediatR;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken
{
    public class GetNewTokenQuery : IRequest<ResponseDto<EmployeeTokenDto>>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Guid? SessionId { get; set; }

        public string Device { get; private set; }

        public GetNewTokenQuery(RequestDto<RequestNewTokenDto> dto)
        {
            Device = dto.DeviceId;
            Username = dto.Payload.Username;
            Password = dto.Payload.Password;
        }

        public GetNewTokenQuery(IRequestDto dto, Guid sessionid)
        {
            Device = dto.DeviceId;
            SessionId = sessionid;
        }
    }
}