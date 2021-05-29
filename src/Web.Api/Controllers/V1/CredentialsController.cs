using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using MediatR;
using MySystem.SharedKernel.Dto.V1;
using MySystem.Web.Api.Security.Policy;
using MySystem.SharedKernel.ExtensionMethod;
using Application.Employee.Command.LoginEmployee.V1;

namespace MySystem.Web.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CredentialsController : Controller
    {
        private readonly IMediator mediatR;
        private readonly ILogger<CredentialsController> logger;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        public CredentialsController(
            IMediator employeeContext,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<CredentialsController> logger)
        {
            this.logger = logger;
            this.mediatR = employeeContext;
            this.configuration = configuration;
            httpContext = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<string>>> LoginV1([FromBody] LoginEmployeeCommand command)
        {
            var response = await mediatR.Send(command);

            return Ok(response);
        }

        [Authorize(Policy = SessionNotBlockedPolicy.Name)]
        [HttpPost("token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ResponseDto<string>>> RefreshTokenV1([FromBody] RequestDto<object> requestDto)
        {
            //var session = Guid.Parse(httpContext.HttpContext.Items[UserClaim.Session] as string);

            //var claims = await mediatR.GetUserClaimsBySessionIdAsync(session);
            //if (claims == null)
            //{
            //    return Unauthorized();
            //}

            //var token = new JwtService(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddDays(7));
            //return Ok(new ResponseDto<string>(token));
            throw new NotImplementedException();
        }

        [Authorize(Policy = TokenRefreshmentPolicy.Name)]
        [HttpPost("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> LogoutV1([FromBody] RequestDto<object> requestDto)
        {
            //var session = Guid.Parse(httpContext.HttpContext.Items[UserClaim.Session] as string);
            //await mediatR.TerminateSessionAsync(session);
            return Ok();
        }
    }
}
