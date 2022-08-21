using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.ApiApp.Application.Contexts.Essential.Commands;
using WeeControl.ApiApp.Application.Contexts.Essential.Queries;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Contexts.Essential.DataTransferObjects.User;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.ApiApp.WebApi.Controllers.Essentials;

[ApiController]
public abstract class UserController : Controller
{
    protected readonly IMediator Mediator;

    protected UserController(IMediator mediator)
    {
        this.Mediator = mediator;
    }

    [AllowAnonymous]
    [HttpHead("{parameter}/{value}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Conflict)]
    public async Task<ActionResult> VerifyNotDuplicate(string parameter, string value)
    {
        var query = new UserDuplicationQuery(parameter, value);
        await Mediator.Send(query);
        return Ok();
    }
    
    [Authorize]
    [HttpPatch(Api.Essential.User.ServerEndPoints.Password)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> SetNewPasswordV1([FromBody] RequestDto<UserPasswordChangeRequestDto> dto)
    {
        var command = new UserNewPasswordCommand(dto, dto.Payload.OldPassword, dto.Payload.NewPassword);
        var response = await Mediator.Send(command);

        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost(Api.Essential.User.ServerEndPoints.Password)]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> ResetPasswordV1([FromBody] RequestDto<UserPasswordResetRequestDto> dto)
    {
        var command = new UserForgotMyPasswordCommand(dto);
        await Mediator.Send(command);
        
        return Ok();
        throw new NotImplementedException();
    }
    
    [HttpGet(Api.Essential.User.ServerEndPoints.Notification)]
    [MapToApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ResponseDto<IEnumerable<UserNotificationDto>>>> GetNotification()
    {
        throw new NotImplementedException();
        // var query = new NotificationQuery();
        // var response = await mediator.Send(query);
        // return Ok(response);
    }

    [HttpDelete(Api.Essential.User.ServerEndPoints.Notification + "/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult> MuteNotificationV1(Guid id)
    {
        var command = new NotificationViewedCommand(id);
        await Mediator.Send(command);
        return Ok();
    }
}