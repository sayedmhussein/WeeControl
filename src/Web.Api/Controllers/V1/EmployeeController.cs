using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Employee.Command.GetRefreshedToken.V1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.AddEmployee.V1;
using MySystem.Application.Employee.Command.AddEmployeeSession.V1;
using MySystem.Application.Employee.Command.TerminateSession.V1;
using MySystem.Application.Employee.Query.GetEmployeeClaims.V1;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Interfaces;
using MySystem.Web.Api.Security.Policies.Employee;

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
        [HttpGet]
        public async Task<ResponseDto<IEnumerable<EmployeeDto>>> GetEmployeesV1(Guid? territoryid, Guid? employeeid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Create New Employee Or Update Current Employee Within the Organization
        /// </summary>
        /// 
        /// <remarks>
        /// Authorized User:
        ///
        ///     1. Must provide brear token issued 15 minutes ago or less.
        ///     2. Only Human Resources Employee who has claim tag "add" and working in office which belong to the added employee or higher.
        ///
        /// </remarks>
        /// 
        /// <param name="command">
        ///     Employee DTO as payload inside Request DTO
        /// </param>
        /// 
        /// <returns>
        ///     The newely created Employee DTO
        /// </returns>
        /// 
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">DTO has missing/invalid values</response>  
        [Authorize(Policy = AbleToAddNewEmployeePolicy.Name)]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeDto>),(int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> PutEmployeeV1([FromBody] AddEmployeeCommand command)
        {
            var response = await mediatR.Send(command);
            return Created("Api/[controller]/" + response.Payload.Id, response);
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

        #region Employee Session
        [AllowAnonymous]
        [HttpPost("Session")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeTokenDto>>> LoginV1([FromBody] AddEmployeeSessionCommand command)
        {
            var response = await mediatR.Send(command);
            var token = jwtService.GenerateJwtToken(response, "", DateTime.UtcNow.AddMinutes(5));
            return Ok(new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto() { Token = token }));
        }

        [Authorize(Policy = AbleToRefreshTokenPolicy.Name)]
        [HttpPut("Session")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeTokenDto>>> RefreshTokenV1([FromBody] GetRefreshedTokenQuery query)
        {
            var sessionid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == Claims.Types[Claims.ClaimType.Session])?.Value;
            var claims = await mediatR.Send(new GetEmployeeClaimsQuery() { SessionId = Guid.Parse(sessionid) });

            var response = await mediatR.Send(query);
            var token = jwtService.GenerateJwtToken(response, "", DateTime.UtcNow.AddDays(5));
            return Ok(new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto() { Token = token }));
        }

        //[Authorize(Policy = TokenRefreshmentPolicy.Name)]
        [HttpDelete("Session")]
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
        #endregion

        #region Employee Claim
        [HttpGet("Claim/{employeeid}")]
        public async Task<ActionResult<IEnumerable<EmployeeClaimDto>>> GetEmployeeClaimsV1(Guid employeeid)
        {
            var response = await mediatR.Send(new GetEmployeeClaimsQuery() { EmployeeId = employeeid });
            var claims = new List<EmployeeClaimDto>();
            foreach (var claim in response)
            {
                claims.Add(new EmployeeClaimDto() { });
            }

            return Ok(new ResponseDto<IEnumerable<EmployeeClaimDto>>(claims));
        }

        [HttpPut("Claim")]
        public async Task<ActionResult> PutEmployeeClaimV1()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("Claim/{claimid}")]
        public async Task<ActionResult> PutEmployeeClaimV1(Guid claimid)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Employee Indentity
        [HttpGet("Identity/{employeeid}")]
        public async Task<ActionResult> GetEmployeeIdentitiesV1(Guid employeeid)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Identity/{employeeid}/{attachmentid}")]
        public async Task<ActionResult> DownloadEmployeeIdentityAttachmentV1(Guid employeeid, Guid attachmentid)
        {
            Stream stream = null;// await { { __get_stream_based_on_id_here__} }

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream"); // returns a FileStreamResult
        }

        [HttpPut("Identity/{employeeid}")]
        public async Task<ActionResult> PutEmployeeIdentity(Guid employeeid)
        {
            throw new NotImplementedException();
        }

        [HttpPut("Identity/{employeeid}/Attachment")]
        //[Consumes("multipart / form - data")]
        public async Task<ActionResult> PutEmployeeIdentity(Guid employeeid, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("Identity/{employeeid}/{identityid}")]
        public async Task<ActionResult> DeleteEmployeeIdentityV1(Guid employeeid, Guid identityid)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("Identity/{employeeid}/Attachment/{attachmentid}")]
        public async Task<ActionResult> DeleteEmployeeIdentityAttachmentV1(Guid employeeid, Guid attachmentid)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
