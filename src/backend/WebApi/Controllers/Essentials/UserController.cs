using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.WebApi.Security.Policies;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Route(Api.Essential.User.Route)]
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
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<ResponseDto<TokenDtoV1>>> RegisterV1([FromBody] RequestDto<UserRegisterDto> dto)
    {
        var command = new UserRegisterCommand(dto);
        var response = await mediator.Send(command);

        return Ok(response);
    }
    
    [Authorize(Policy = nameof(CanEditUserPolicy))]
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResponseDto<IEnumerable<UserDtoV1>>>> GetListOfUsersV1()
    {
        var command = new UserQuery();
        var response = await mediator.Send(command);

        return Ok(response);
    }
    
    [Authorize]
    [HttpGet("{username}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResponseDto<IEnumerable<UserDtoV1>>>> GetUserDetailsV1(string username)
    {
        var command = new GetUserDetailsQuery(username);
        var response = await mediator.Send(command);

        return Ok(response);
    }

    [Authorize]
    [HttpPatch(Api.Essential.User.ResetPassword)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> SetNewPasswordV1([FromBody] RequestDto<SetNewPasswordDtoV1> dto)
    {
        var command = new UserNewPasswordCommand(dto, dto.Payload.OldPassword, dto.Payload.NewPassword);
        var response = await mediator.Send(command);

        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost(Api.Essential.User.ResetPassword)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> ResetPasswordV1([FromBody] RequestDto<ForgotMyPasswordDtoV1> dto)
    {
        var command = new UserForgotMyPasswordCommand(dto);
        await mediator.Send(command);

        return Ok();
    }
}