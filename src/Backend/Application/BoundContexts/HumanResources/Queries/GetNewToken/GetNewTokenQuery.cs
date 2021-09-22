using System;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Authorization;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken
{
    public class GetNewTokenQuery : IRequest<ResponseDto<EmployeeTokenDto>>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Guid? SessionId { get; set; }

        public int Otp { get; private set; }

        public string Device { get; private set; }

        public GetNewTokenQuery(RequestDto<RequestNewTokenDto> dto)
        {
            Device = dto.DeviceId;
            Username = dto.Payload.Username;
            Password = dto.Payload.Password;
        }

        public GetNewTokenQuery(IRequestDto dto, int otp)
        {
            Device = dto.DeviceId;
            Otp = otp;
        }

        public GetNewTokenQuery(IRequestDto dto, Guid sessionid)
        {
            Device = dto.DeviceId;
            SessionId = sessionid;
        }
    }
}