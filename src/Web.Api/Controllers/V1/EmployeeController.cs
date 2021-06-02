using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Employee.Command.GetRefreshedToken.V1;
using Application.Employee.Query.GetNewToken.V1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySystem.Application.Employee.Command.TerminateSession.V1;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.Interfaces;
using MySystem.Web.Api.Security.Policy;

namespace MySystem.Web.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IMediator mediatR;

        public EmployeeController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        [AllowAnonymous]
        [HttpPost("Credentials/login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IResponseDto<>), (int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<string>>> LoginV1([FromBody] GetNewTokenQuery query)
        {
            var response = await mediatR.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = SessionNotBlockedPolicy.Name)]
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

        [Authorize(Policy = TokenRefreshmentPolicy.Name)]
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
