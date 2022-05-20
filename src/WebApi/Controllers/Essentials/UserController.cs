using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.EssentialContext.Commands;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
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
    [HttpPost(Api.Essential.User.Base)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<ResponseDto<TokenDtoV1>>> RegisterV1([FromBody] RequestDto<RegisterDtoV1> dto)
    {
        var command = new RegisterCommand(dto, dto.Payload);
        var response = await mediator.Send(command);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Api.Essential.User.Session)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<TokenDtoV1>>> LoginV1([FromBody] RequestDto<LoginDtoV1> dto)
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
    [HttpPut(Api.Essential.User.Session)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto<TokenDtoV1>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto<TokenDtoV1>>> RefreshTokenV1([FromBody] RequestDto dto)
    {
        var query = new GetNewTokenQuery(dto);
        var response = await mediator.Send(query);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete(Api.Essential.User.Session)]
    [MapToApiVersion("1.0")]
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
    [HttpPatch(Api.Essential.User.Reset)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto>> UpdatePasswordV1([FromBody] RequestDto<MeForgotPasswordDtoV1> dto)
    {
        var command = new UpdatePasswordCommand(dto, dto.Payload.OldPassword, dto.Payload.NewPassword);
        var response = await mediator.Send(command);

        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost(Api.Essential.User.Reset)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ResponseDto>> ResetPasswordV1([FromBody] RequestDto<PutNewPasswordDtoV1> dto)
    {
        var command = new ResetPasswordCommand(dto, dto.Payload.Email, dto.Payload.Username);
        await mediator.Send(command);

        return Ok();
    }
}