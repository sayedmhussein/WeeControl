using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Backend.Application.Activities.Employee.Commands.TerminateSessionV1;
using WeeControl.Backend.Application.Activities.Employee.Queries.GetClaimsV1;
using WeeControl.Backend.Application.Activities.Employee.Queries.GetTokenDtoV1;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Authorization;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;
using WeeControl.Common.SharedKernel.Extensions;

namespace WeeControl.Backend.WebApi.Controllers.Authorization
{
    public partial class AuthorizationController
    {
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
            if (dto.IsValid() == false)
                return BadRequest();

            if (dto.Payload is RequestNewTokenDto == false)
                return BadRequest();

            var query = new GetEmployeeClaimsQuery() { Username = dto.Payload.Username, Password = dto.Payload.Password, Device = dto.DeviceId };
            var response = await GetResponseTokenDto(query, DateTime.UtcNow.AddMinutes(5));

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
        public async Task<ActionResult<ResponseDto<EmployeeTokenDto>>> RefreshTokenV1([FromBody] RequestDto<RefreshLoginDto> dto)
        {
            var query = new GetEmployeeClaimsQuery() { Device = dto.DeviceId };
            var response = await GetResponseTokenDto(query, DateTime.UtcNow.AddDays(5));

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
            var command = new TerminateSessionCommand();
            await mediatR.Send(command);
            return Ok();
        }

        private async Task<ResponseDto<EmployeeTokenDto>> GetResponseTokenDto(GetEmployeeClaimsQuery query, DateTime validity)
        {
            var claims = await mediatR.Send(query);
            var token = jwtService.GenerateJwtToken(claims, "WeeControl", validity);
            var value = await mediatR.Send(new GetTokenQuery(token));
            var response = new ResponseDto<EmployeeTokenDto>(value);
            return response;
        }
        #endregion
    }
}