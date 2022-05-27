using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.EssentialContext.Commands;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
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

    [Authorize]
    [HttpPatch(Api.Essential.User.Reset)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto>> UpdatePasswordV1([FromBody] RequestDto<SetNewPasswordDtoV1> dto)
    {
        var command = new UpdatePasswordCommand(dto, dto.Payload.OldPassword, dto.Payload.NewPassword);
        var response = await mediator.Send(command);

        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost(Api.Essential.User.Reset)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ResponseDto>> ResetPasswordV1([FromBody] RequestDto<ForgotMyPasswordDto> dto)
    {
        var command = new ResetPasswordCommand(dto, dto.Payload.Email, dto.Payload.Username);
        await mediator.Send(command);

        return Ok();
    }
}