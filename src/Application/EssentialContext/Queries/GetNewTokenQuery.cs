using System;
using MediatR;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Queries;

public class GetNewTokenQuery : IRequest<ResponseDto<TokenDtoV1>>
{
    public GetNewTokenQuery(IRequestDto request)
    {
        Request = request;
    }

    public GetNewTokenQuery(IRequestDto<LoginDtoV1> dto)
    {
        Request = dto;
        Payload = LoginDtoV1.Create(dto.Payload.UsernameOrEmail.ToLower(), dto.Payload.Password);
    }
        
    [Obsolete("Use other constructor which has one argument.")]
    public GetNewTokenQuery(IRequestDto request, LoginDtoV1 payload)
    {
        Request = request;
        Payload = payload;

        Payload.UsernameOrEmail = payload.UsernameOrEmail.ToLower();
    }

    public IRequestDto Request { get; }
    public LoginDtoV1 Payload { get; }
}