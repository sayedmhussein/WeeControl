using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.EssentialContext.Commands;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Route(Api.Essential.Authorization.Root)]
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
    [HttpPut]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto<TokenDtoV1>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<ResponseDto<TokenDtoV1>>> RefreshTokenV1([FromBody] RequestDto dto)
    {
        var query = new GetNewTokenQuery(dto);
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
        var command = new LogoutCommand(dto);
        var response = await mediator.Send(command);

        return NotFound(response);
    }
}