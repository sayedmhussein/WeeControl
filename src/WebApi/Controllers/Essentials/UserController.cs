using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.EssentialContext.Commands;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Essential.ResponseDTOs;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : Controller
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost(RegisterDto.HttpPostMethod.EndPoint)]
    [MapToApiVersion(RegisterDto.HttpPostMethod.Version)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<ResponseDto<TokenDto>>> RegisterV1([FromBody] RequestDto<RegisterDto> dto)
    {
        var command = new RegisterCommand(dto, dto.Payload);
        var response = await mediator.Send(command);

        return Ok(new ResponseDto<TokenDto>(response));
    }

    [AllowAnonymous]
    [HttpPost(TokenDto.HttpPostMethod.EndPoint)]
    [MapToApiVersion(TokenDto.HttpPostMethod.Version)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<TokenDto>>> LoginV1([FromBody] RequestDto<LoginDto> dto)
    {
        var query = new GetNewTokenQuery(dto);
        var response = await mediator.Send(query);

        return Ok(response);
    }

    /// <summary>
    ///     Used to get token which will be used to authorize user, device must match the same which had the temporary token.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut(TokenDto.HttpPutMethod.EndPoint)]
    [MapToApiVersion(TokenDto.HttpPutMethod.Version)]
    [ProducesResponseType(typeof(ResponseDto<TokenDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto<TokenDto>>> RefreshTokenV1([FromBody] RequestDto dto)
    {
        var query = new GetNewTokenQuery(dto);
        var response = await mediator.Send(query);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete(TokenDto.HttpDeleteMethod.EndPoint)]
    [MapToApiVersion(TokenDto.HttpDeleteMethod.Version)]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto>> LogoutV1([FromBody] RequestDto dto)
    {
        var command = new LogoutCommand(dto);
        var response = await mediator.Send(command);

        return Ok(response);
    }

    [Authorize]
    [HttpPatch(PasswordSetForgottenDto.HttpPatchMethod.EndPoint)]
    [MapToApiVersion(PasswordSetForgottenDto.HttpPatchMethod.Version)]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto>> UpdatePasswordV1([FromBody] RequestDto<PasswordSetForgottenDto> dto)
    {
        var command = new UpdatePasswordCommand(dto, dto.Payload.OldPassword, dto.Payload.NewPassword);
        var response = await mediator.Send(command);

        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost(PasswordResetRequestDto.HttpPostMethod.EndPoint)]
    [MapToApiVersion(PasswordResetRequestDto.HttpPostMethod.Version)]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ResponseDto>> ResetPasswordV1([FromBody] RequestDto<PasswordResetRequestDto> dto)
    {
        var command = new ResetPasswordCommand(dto, dto.Payload.Email, dto.Payload.Username);
        await mediator.Send(command);

        return Ok();
    }
}