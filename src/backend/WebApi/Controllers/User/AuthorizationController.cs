using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApi.Controllers.User;

[ApiController]
[Route(ApiRouting.AuthorizationRoute)]
[ProducesResponseType((int)HttpStatusCode.BadRequest)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class AuthorizationController : Controller
{
    private readonly IMediator mediator;

    public AuthorizationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpHead]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public ActionResult Ping()
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<TokenResponseDto>>> LoginV1([FromBody] RequestDto<LoginRequestDto> dto)
    {
        var response = await mediator.Send(new SessionCreateCommand(dto));
        return Ok(response);
    }

    [Authorize]
    [HttpPut("{otp}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto<TokenResponseDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto<TokenResponseDto>>> RefreshTokenOtp(string otp, [FromBody] RequestDto dto)
    {
        var response = await mediator.Send(new SessionUpdateCommand(dto, otp));
        return Ok(response);
    }

    [Authorize]
    [HttpPatch]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto<TokenResponseDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto<TokenResponseDto>>> RefreshTokenV1([FromBody] RequestDto dto)
    {
        var response = await mediator.Send(new SessionUpdateCommand(dto));
        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<ResponseDto>> LogoutV1([FromBody] RequestDto dto)
    {
        var response = await mediator.Send(new SessionTerminateCommand(dto));
        return Ok(response);
    }
}