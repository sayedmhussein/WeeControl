﻿using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Employee.Command.GetRefreshedToken.V1;
using Application.Employee.Query.GetNewToken.V1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.AddEmployee.V1;
using MySystem.Application.Employee.Command.AddEmployeeSession.V1;
using MySystem.Application.Employee.Command.TerminateSession.V1;
using MySystem.Application.Employee.Query.GetEmployeeTerritories.V1;
using MySystem.SharedKernel.Entites.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Interfaces;
using MySystem.Web.Api.Security.Policy;
using MySystem.Web.Api.Security.Policy.Employee;

namespace MySystem.Web.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IMediator mediatR;
        private readonly IJwtService jwtService;

        public EmployeeController(IMediator mediatR, IJwtService jwtService)
        {
            this.mediatR = mediatR;
            this.jwtService = jwtService;
        }

        #region Employee
        [Authorize(Policy = AbleToAddNewEmployeePolicy.Name)]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> AddEmployeeV1([FromBody] AddEmployeeCommand command)
        {
            var response = await mediatR.Send(command);
            return Created("Api/[controller]/" + response.Payload.Id, response);
        }

        [Authorize(Policy = AbleToEditExisingEmployeePolicy.Name)]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> EditEmployeeV1([FromBody] AddEmployeeCommand command)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy = AbleToDeleteExisingEmployeePolicy.Name)]
        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> DeleteEmployeeV1(Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Session
        #endregion


        [HttpGet("Territories")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<EmployeeTerritoriesDto>> GetTerritoriesV1(Guid? employeeid, Guid? sessionid)
        {
            var response = await mediatR.Send(new GetEmployeeTerritoriesQuery() { EmployeeId = employeeid, SessionId = sessionid });
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Credentials/login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<string>>> LoginV1([FromBody] AddEmployeeSessionCommand command)
        {
            var response = await mediatR.Send(command);
            var token = jwtService.GenerateJwtToken(response, "", DateTime.UtcNow.AddMinutes(5));
            return Ok(token);
        }

        //[Authorize(Policy = SessionNotBlockedPolicy.Name)]
        [HttpPost("Credentials/token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<string>>> RefreshTokenV1([FromBody] GetRefreshedTokenQuery query)
        {
            var response = await mediatR.Send(query);
            return Ok(response);
        }

        //[Authorize(Policy = TokenRefreshmentPolicy.Name)]
        [HttpPost("Credentials/logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> LogoutV1([FromBody] TerminateSessionCommand command)
        {
            await mediatR.Send(command);
            return Ok();
        }
    }
}
