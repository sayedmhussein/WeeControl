using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.LogoutEmployee;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;

namespace WeeControl.Backend.WebApi.Controllers.HumanResources
{
    [Route(ApiRouteLink.HumanResources.Authorization.Route)]
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
        [HttpPost(ApiRouteLink.HumanResources.Authorization.RequestNewToken.EndPoint)]
        [MapToApiVersion(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Version)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
        [HttpPut(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.EndPoint)]
        [MapToApiVersion(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ResponseDto<EmployeeTokenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
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
        [Authorize]
        [HttpDelete(ApiRouteLink.HumanResources.Authorization.Logout.EndPoint)]
        [MapToApiVersion(ApiRouteLink.HumanResources.Authorization.Logout.Version)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ResponseDto>> LogoutV1([FromBody] RequestDto dto)
        {
            var command = new LogoutEmployeeCommand(dto.DeviceId, currentUserInfo.GetSessionId());
            var response = await mediatR.Send(command);
            return Ok(response);
        }
        #endregion
    }
}