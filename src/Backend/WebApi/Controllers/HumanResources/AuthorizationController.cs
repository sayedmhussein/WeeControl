using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;

namespace WeeControl.Backend.WebApi.Controllers.HumanResources
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IMediator mediatR;
        private readonly ICurrentUserInfo currentUserInfo;

        public AuthorizationController(IMediator mediatR, ICurrentUserInfo currentUserInfo)
        {
            this.mediatR = mediatR;
            this.currentUserInfo = currentUserInfo;
        }
        
        #region Authorization
        /// <summary>
        ///     User can get a temporary token by supplying username and password.
        /// </summary>
        /// <param name="dto">Payloading object that contains username and password.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeTokenDto>>> LoginV1([FromBody] RequestDto<RequestNewTokenDto> dto)
        {
            if (dto.Payload is null)
                return BadRequest();

            var query = new GetNewTokenQuery(dto);
            var response = await mediatR.Send(query);

            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpPost("forgot")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public Task<ActionResult<ResponseDto<EmployeeTokenDto>>> ResetPasswordV1([FromBody] RequestDto<RequestPasswordResetDto> dto)
        {
            throw new NotImplementedException();
        }
        
        [AllowAnonymous]
        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public Task<ActionResult<ResponseDto<EmployeeTokenDto>>> SetNewPasswordV1([FromBody] RequestDto<SetNewPasswordDto> dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Used to get token which will be used to authorize user, device must match the same which had the temporary token.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<EmployeeTokenDto>>> RefreshTokenV1([FromBody] RequestDto dto)
        {
            var query = new GetNewTokenQuery(dto, currentUserInfo.GetSessionId());
            var response = await mediatR.Send(query);

            return Ok(response);
        }

        /// <summary>
        ///     Used to terminate user session using token.
        /// </summary>
        /// <returns></returns>
        [Authorize()]
        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> LogoutV1()
        {
            // var command = new TerminateSessionCommand();
            // await mediatR.Send(command);
            // return Ok();
            throw new NotImplementedException();
        }
        #endregion
    }
}