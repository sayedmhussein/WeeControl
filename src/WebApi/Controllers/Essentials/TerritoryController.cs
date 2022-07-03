using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.Essential.Commands;
using WeeControl.Application.Essential.Queries;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Authorize]
[Route(Api.Essential.Territory.EndPoint)]
[ProducesResponseType((int)HttpStatusCode.OK)]
[ProducesResponseType((int)HttpStatusCode.Forbidden)]
[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
public class TerritoryController : Controller
{
    private readonly IMediator mediator;

    public TerritoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResponseDto<IEnumerable<TerritoryDto>>>> GetListOfTerritoriesV1()
    {
        var response = await mediator.Send(new GetListOfTerritoriesQuery());
        return Ok(response);
    }
    
    [HttpPut]
    [MapToApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult> AddOrEditTerritoryV1([FromBody] RequestDto<TerritoryDto> dto)
    {
        await mediator.Send(new AddOrEditTerritoryCommand(dto));
        return Ok();
    }
}