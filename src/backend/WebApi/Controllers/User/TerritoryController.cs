// using System;
// using System.Collections.Generic;
// using System.Net;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using WeeControl.ApiApp.Application.Contexts.Essential.Queries;
// using WeeControl.Common.SharedKernel;
// using WeeControl.Common.SharedKernel.Contexts.Temporary.DataTransferObjects;
// using WeeControl.Common.SharedKernel.RequestsResponses;
//
// namespace WeeControl.ApiApp.WebApi.Controllers.Essentials;
//
// [ApiController]
// [Authorize]
// [Route(ApiRouting.Essential.Routes.Territory)]
// [ProducesResponseType((int)HttpStatusCode.OK)]
// [ProducesResponseType((int)HttpStatusCode.Forbidden)]
// [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
// public class TerritoryController : Controller
// {
//     private readonly IMediator mediator;
//
//     public TerritoryController(IMediator mediator)
//     {
//         this.mediator = mediator;
//     }
//     
//     [AllowAnonymous]
//     [HttpGet]
//     [MapToApiVersion("1.0")]
//     public async Task<ActionResult<ResponseDto<IEnumerable<TerritoryDto>>>> GetListOfTerritoriesV1()
//     {
//         var response = await mediator.Send(new TerritoryQuery());
//         return Ok(response);
//     }
//     
//     [HttpPut]
//     [MapToApiVersion("1.0")]
//     [ProducesResponseType((int)HttpStatusCode.Conflict)]
//     public Task<ActionResult> AddOrEditTerritoryV1([FromBody] RequestDto<TerritoryDto> dto)
//     {
//         throw new NotImplementedException();
//     }
// }