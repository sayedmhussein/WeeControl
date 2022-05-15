using System;
using MediatR;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Essential.ResponseDTOs;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Queries;

public class GetNewTokenQuery : IRequest<ResponseDto<TokenDto>>
{
    public GetNewTokenQuery(IRequestDto request)
    {
        Request = request;
    }

    public GetNewTokenQuery(IRequestDto<LoginDto> dto)
    {
        Request = dto;
        Payload = new LoginDto(dto.Payload.UsernameOrEmail.ToLower(), dto.Payload.Password);
    }
        
    [Obsolete("Use other constructor which has one argument.")]
    public GetNewTokenQuery(IRequestDto request, LoginDto payload)
    {
        Request = request;
        Payload = payload;

        Payload.UsernameOrEmail = payload.UsernameOrEmail.ToLower();
    }

    public IRequestDto Request { get; }
    public LoginDto Payload { get; }
}