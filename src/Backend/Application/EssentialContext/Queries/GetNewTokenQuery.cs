using System;
using MediatR;
using WeeControl.Common.SharedKernel.Essential.RequestDTOs;
using WeeControl.Common.SharedKernel.Essential.ResponseDTOs;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Backend.Application.EssentialContext.Queries;

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