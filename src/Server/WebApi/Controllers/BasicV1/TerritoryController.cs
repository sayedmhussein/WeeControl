using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Server.Application.Basic.Territory.V1.Commands;
using WeeControl.Server.Application.Territory.V1.Commands;
using WeeControl.Server.Application.Territory.V1.Queries;
using WeeControl.Server.WebApi.Security.Policies;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Entities.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Territory.Entities.DtosV1;

namespace WeeControl.Server.WebApi.Controllers.BasicV1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(Policy = BasicPolicies.HasActiveSession)]
    [ApiController]
    public class TerritoryController : Controller
    {
        private readonly IMediator mediatR;

        public TerritoryController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        /// <summary>
        ///     Get List of Territories in Organization
        /// </summary>
        /// <param name="id">Optional to get the children of the supplied territory id</param>
        /// <returns>List of Territory DTOs</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TerritoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<TerritoryDto>>> GetAllTerritoriesV1(Guid? id)
        {
            var response =
                await mediatR.Send(new GetTerritoriesQuery()
                {
                    TerritoryId = id
                });

            return Ok(response);
        }

        /// <summary>
        /// Insert or update territory within the organization
        /// </summary>
        /// <param name="requestDto">The territory DTO, if ID was supplied then this will be update</param>
        /// <returns>Insert a territory DTO</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = BasicPolicies.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TerritoryDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<TerritoryDto>>> AddTerritoryV1([FromBody] TerritoryDto requestDto)
        {
            await mediatR.Send(new AddTerritoryCommand() { TerritoryDto = requestDto });

            return Created("", null);
        }

        /// <summary>
        /// Insert or update territory within the organization
        /// </summary>
        /// <param name="requestDto">The territory DTO, if ID was supplied then this will be update</param>
        /// <returns>If insert then it will return the territory DTO</returns>
        [HttpPut]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = BasicPolicies.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TerritoryDto>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<TerritoryDto>>> PutTerritoryV1([FromBody] TerritoryDto requestDto)
        {
            await mediatR.Send(new UpdateTerritoryCommand() { TerritoryDto = requestDto });

            return NoContent();
        }

        /// <summary>
        /// Delete existing territory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{territoryid}")]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = BasicPolicies.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> DeleteTerritoryV1(Guid id)
        {
            await mediatR.Send(new DeleteTerritoriesCommand()
            {
                TerritoryIds = new List<Guid>() { id }
            });

            return NoContent();
        }
    }
}
