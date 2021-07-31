using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Server.Application.Basic.Territory.Commands.AddTerritoryV1;
using WeeControl.Server.Application.Territory.Commands.DeleteTerritoriesV1;
using WeeControl.Server.Application.Territory.Commands.UpdateTerritoryV1;
using WeeControl.Server.Application.Territory.Queries.GetTerritoryV1;
using WeeControl.Server.WebApi.Security.Policies;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Entities.DtosV1;
using WeeControl.SharedKernel.Dtos.V1;

namespace WeeControl.Server.WebApi.Controllers.Territory
{
    public partial class TerritoryController
    {
        /// <summary>
        ///     Get List of Territories in Organization
        /// </summary>
        /// <param name="id">Optional to get the children of the supplied territory id</param>
        /// <returns>List of Territory DTOs</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<TerritoryWithIdDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<ResponseDto<IEnumerable<TerritoryWithIdDto>>>> GetAllTerritoriesV1(Guid? id)
        {
            var query = new GetTerritoriesQuery(id);
            var value = await mediatR.Send(query);
            var response = new ResponseDto<IEnumerable<TerritoryWithIdDto>>(value);

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
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddTerritoriesV1([FromBody] RequestDto<IEnumerable<TerritoryDto>> requestDto)
        {
            var command = new AddTerritoryCommand() { TerritoryDtos = requestDto.Payload };
            await mediatR.Send(command);

            return Created("", null);
        }

        /// <summary>
        /// Insert or update territory within the organization
        /// </summary>
        /// <param name="requestDto">The territory DTO, if ID was supplied then this will be update</param>
        /// <returns>If insert then it will return the territory DTO</returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = BasicPolicies.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorSimpleDetailsDto), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> PutTerritoryV1(Guid id, [FromBody] RequestDto<TerritoryDto> requestDto)
        {
            var command = new UpdateTerritoryCommand(id, requestDto.Payload);
            await mediatR.Send(command);

            return NoContent();
        }

        /// <summary>
        ///     Delete existing territory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
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
            var command = new DeleteTerritoryCommand(id);
            await mediatR.Send(command);

            return NoContent();
        }
    }
}
