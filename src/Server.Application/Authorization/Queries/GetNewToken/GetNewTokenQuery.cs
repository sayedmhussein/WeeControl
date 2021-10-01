using System;
using MediatR;
using WeeControl.SharedKernel.Authorization.DtosV1;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.Application.Authorization.Queries.GetNewToken
{
    public class GetNewTokenQuery : IRequest<ResponseDto<EmployeeTokenDto>>
    {
        public RequestNewTokenDto RequestNewTokenDto { get; }

        public Guid? SessionId { get; }

        public string Device { get; }

        public GetNewTokenQuery(RequestDto<RequestNewTokenDto> dto)
        {
            Device = dto.DeviceId;
            RequestNewTokenDto = dto.Payload;
        }

        public GetNewTokenQuery(IRequestDto dto, Guid? sessionid)
        {
            Device = dto.DeviceId;
            SessionId = sessionid;
        }
    }
}