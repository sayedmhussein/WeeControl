using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Contexts.System.Commands;
using WeeControl.SharedKernel;
using WeeControl.WebApi.Security.Policies;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Authorize]
[ProducesResponseType((int)HttpStatusCode.OK)]
[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
[ProducesResponseType((int)HttpStatusCode.Forbidden)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class AdminController : Controller
{
    private readonly IMediator mediator;

    public AdminController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Authorize(Policy = DeveloperWithDatabaseOperationPolicy.Name)]
    [HttpHead(Api.Essential.Admin.Route)]
    public async Task<ActionResult> PopulateDatabase()
    {
        await mediator.Send(new SeedEssentialDatabaseCommand());
        return Ok();
    }
    
    
}