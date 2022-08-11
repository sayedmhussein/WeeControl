using System;
using System.Collections.Generic;
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

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Authorize]
[Route(Api.Essential.Notification.Route)]
[Consumes(MediaTypeNames.Application.Json)]
public class NotificationController : Controller
{
    private readonly IMediator mediator;

    public NotificationController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    // [HttpGet]
    // [MapToApiVersion("1.0")]
    // [Produces(MediaTypeNames.Application.Json)]
    // public async Task<ActionResult<ResponseDto<IEnumerable<NotificationDto>>>> GetNotification()
    // {
    //     var query = new NotificationQuery();
    //     var response = await mediator.Send(query);
    //     return Ok(response);
    // }

    [HttpDelete("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult> MuteNotificationV1(Guid id)
    {
        var command = new NotificationViewedCommand(id);
        await mediator.Send(command);
        return Ok();
    }
}