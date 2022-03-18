using System;
using MediatR;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Queries
{
    public class GetNewTokenQuery : IRequest<ResponseDto<TokenDto>>
    {
        public GetNewTokenQuery(RequestDto request, LoginDto payload)
        {
            Request = request;
            Payload = payload;
        }

        public GetNewTokenQuery(RequestDto request, Guid? sessionId)
        {
            Request = request;
            SessionId = sessionId;
        }

        public RequestDto Request { get; }
        public LoginDto Payload { get; }

        public Guid? SessionId { get; set; }
    }
}
