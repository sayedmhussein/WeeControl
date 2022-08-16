using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Route(Api.Essential.Routes.Authorization)]
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
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<AuthenticationResponseDto>>> LoginV1([FromBody] RequestDto<AuthenticationRequestDto> dto)
    {
        var query = new UserTokenQuery(dto);
        var response = await mediator.Send(query);

        return Ok(response);
    }
    
    [Authorize]
    [HttpPut]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto<AuthenticationResponseDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto<AuthenticationResponseDto>>> RefreshTokenV1([FromBody] RequestDto dto)
    {
        var query = new UserTokenQuery(dto);
        var response = await mediator.Send(query);

        return Ok(response);
    }
    
    [Authorize]
    [HttpDelete]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<ResponseDto>> LogoutV1([FromBody] RequestDto dto)
    {
        var command = new UserLogoutCommand(dto);
        var response = await mediator.Send(command);

        return NotFound(response);
    }
}