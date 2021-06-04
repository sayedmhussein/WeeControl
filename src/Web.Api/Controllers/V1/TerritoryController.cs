using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySystem.Application.Employee.Query.GetEmployeeTerritories.V1;
using MySystem.SharedKernel.Entites.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.Entities.Public.V1Dto;

namespace MySystem.MySystem.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    //[Authorize(Policy = AbleToAddNewEmployeePolicy.Name)]
    [ApiVersion("1.0")]
    [ApiController]
    public class TerritoryController : Controller
    {
        private readonly IMediator mediatR;

        public TerritoryController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public Task<ActionResult> GetAllTerritoriesV1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Get List of Territoreis which are related to specific employee
        /// </summary>
        ///
        /// <remarks>
        /// Authorized User:
        ///
        ///     1. Must provide brear token issued 15 minutes ago or less.
        ///     2. Only Human Resource User with tag "get" and working in office which belong to the added employee or higher.
        /// 
        /// </remarks>
        /// 
        /// <param name="employeeid">Employee UUID</param>
        /// <param name="sessionid">Employee's session UUID</param>
        /// 
        /// <returns>
        ///     Response DTO with List of EmployeeTerritory DTOs
        /// </returns>
        [HttpGet("Employee")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<EmployeeTerritoriesDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<IEnumerable<EmployeeTerritoriesDto>>>> GetTerritoriesV1(Guid? employeeid, Guid? sessionid)
        {
            var response = await mediatR.Send(new GetEmployeeTerritoriesQuery() { EmployeeId = employeeid, SessionId = sessionid });
            return Ok(response);
        }

        [HttpPut]
        public Task<ActionResult> PutTerritoryV1()
        {
            throw new NotImplementedException();
        }

        [HttpPut("{territoryid}")]
        public Task<ActionResult> DeleteTerritoryV1(Guid territoryid)
        {
            throw new NotImplementedException();
        }
    }
}
