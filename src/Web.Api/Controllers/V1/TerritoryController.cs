using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySystem.Application.Territory.Command.AddTerritory;
using MySystem.Application.Territory.Command.DeleteTerritory;
using MySystem.Application.Territory.Query.GetTerritories;
using MySystem.SharedKernel.Entites.Public.V1Dto;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Entities.Territory.V1Dto;
using MySystem.Web.Api.Security.Policies.Employee;

namespace MySystem.MySystem.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
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
        /// <remarks>
        /// Any user can use this route
        /// </remarks>
        /// <param name="territoryid">Optional to get the children of the supplied territory id</param>
        /// <param name="employeeid">Optional to get the children of the supplied employee id</param>
        /// <param name="sessionid">Optional to get the children of the supplied employee's session id</param>
        /// <returns>List of Territory DTOs</returns>
        [Authorize(AuthenticationSchemes = "Brear")]
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TerritoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<TerritoryDto>>> GetAllTerritoriesV1(Guid? territoryid, Guid? employeeid, Guid? sessionid)
        {
            var response =
                await mediatR.Send(new GetTerritoriesV1Query()
                {
                    TerritoryId = territoryid,
                    EmployeeId = employeeid,
                    SessionId = sessionid
                });

            return Ok(response);
        }

        /// <summary>
        /// Insert or update territory within the organization
        /// </summary>
        /// <param name="requestDto">The territory DTO, if ID was supplied then this will be update</param>
        /// <returns>If insert then it will return the territory DTO</returns>
        //[Authorize(Policy = CanEditTerritoryPolicy.Name)]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TerritoryDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TerritoryDto>> PutTerritoryV1([FromBody] RequestDto<TerritoryDto> requestDto)
        {
            var response =
                await mediatR.Send(new AddTerritoryV1Command()
                {
                    TerritoryDtos = new List<TerritoryDto>()
                    {
                        requestDto.Payload
                    }
                });

            return Created("", response);
        }

        /// <summary>
        /// Delete existing territory
        /// </summary>
        /// <param name="territoryid"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Policy = CanEditTerritoryPolicy.Name)]
        [HttpDelete("{territoryid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteTerritoryV1(Guid territoryid)
        {
            await mediatR.Send(new DeleteTerritoryV1Command()
            {
                TerritoryIds = new List<Guid>()
                {
                    territoryid
                }
            });

            return NoContent();
        }
    }
}
