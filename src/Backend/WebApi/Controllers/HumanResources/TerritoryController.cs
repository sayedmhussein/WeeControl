using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.AddTerritoryV1;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.DeleteTerritoryV1;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.UpdateTerritoryV1;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;

namespace WeeControl.Backend.WebApi.Controllers.HumanResources
{
    [Route("Api/[controller]")]
    [Authorize]
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
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<IdentifiedTerritoryDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>> GetAllTerritoriesV1(Guid? id)
        {
            // var query = new GetTerritoriesQuery(id);
            // var value = await mediatR.Send(query);
            // var response = new ResponseDto<IEnumerable<IdentifiedTerritoryDto>>(value);

            //return Ok(response);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert or update territory within the organization
        /// </summary>
        /// <param name="dto">The territory DTO</param>
        /// <returns>Insert a territory DTO</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        //[Authorize(CustomAuthorizationPolicy.Territory.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddTerritoriesV1([FromBody] RequestDto<TerritoryDto> dto)
        {
            var command = new AddTerritoryCommand(dto);
            var territory = await mediatR.Send(command);

            return Created($"Api/Territory?id={territory.Id}", territory);
        }

        /// <summary>
        /// Insert or update territory within the organization
        /// </summary>
        /// <param name="id">ID of the territory</param>
        /// <param name="requestDto">The territory DTO, if ID was supplied then this will be update</param>
        /// <returns>If insert then it will return the territory DTO</returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        //[Authorize(CustomAuthorizationPolicy.Territory.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> PutTerritoryV1(Guid id, [FromBody] RequestDto<TerritoryDto> requestDto)
        {
            var command = new UpdateTerritoryCommand(id, requestDto.Payload);
            await mediatR.Send(command);

            return NoContent();
        }

        /// <summary>
        ///     Delete existing territory
        /// </summary>
        /// <param name="id">ID of the territory</param>
        /// <param name="dto">Request DTO of type object.</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [MapToApiVersion("1.0")]
        //[Authorize(CustomAuthorizationPolicy.Territory.CanAlterTerritories)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> DeleteTerritoryV1([FromBody] RequestDto<object> dto, Guid id)
        {
            _ = dto;
            var command = new DeleteTerritoryCommand(id);
            await mediatR.Send(command);

            return NoContent();
        }
    }
}
