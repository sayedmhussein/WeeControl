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
using MySystem.Web.Api.Service;
using MySystem.Web.Api.Security.Policy;
using MySystem.Web.Api.Domain.Employee;
using MySystem.Shared.Library.Dto.EntityV1;
using System.Security.Claims;
using MySystem.Shared.Library.Definition;
using MySystem.Shared.Library.ExtensionMethod;

namespace MySystem.Web.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CredentialsController : Controller
    {
        private readonly IEmployeeService employeeContext;
        private readonly ILogger<CredentialsController> logger;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        public CredentialsController(
            IEmployeeService employeeContext,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<CredentialsController> logger)
        {
            this.logger = logger;
            this.employeeContext = employeeContext;
            this.configuration = configuration;
            httpContext = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<string>>> LoginV1([FromBody] RequestDto<LoginDto> requestDto)
        {
            if (requestDto.Payload.IsValid() == false)
            { 
                return BadRequest(requestDto.Payload.ErrorMessage());
            }

            var session = await employeeContext.GetSessionIdAsync(requestDto.Payload.Username, requestDto.Payload.Password, requestDto.DeviceId);
            if (session == null)
            {
                logger?.LogInformation("Invalid Attempt to login with invalid username={0}, password={1}.", requestDto.Payload.Username, requestDto.Payload.Password);
                return NotFound();
            }

            var claims = new List<Claim>()
            {
                new Claim(UserClaim.Session, session.ToString())
            };

            var token = new JwtService(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddMinutes(5));

            return Ok(new ResponseDto<string>(token));
        }

        [Authorize(Policy = SessionNotBlockedPolicy.Name)]
        [HttpPost("token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ResponseDto<string>>> RefreshTokenV1([FromBody] RequestDto<object> requestDto)
        {
            var session = Guid.Parse(httpContext.HttpContext.Items[UserClaim.Session] as string);

            var claims = await employeeContext.GetUserClaimsBySessionIdAsync(session);
            if (claims == null)
            {
                return Unauthorized();
            }

            var token = new JwtService(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddDays(7));
            return Ok(new ResponseDto<string>(token));
        }

        [Authorize(Policy = TokenRefreshmentPolicy.Name)]
        [HttpPost("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> LogoutV1([FromBody] RequestDto<object> requestDto)
        {
            var session = Guid.Parse(httpContext.HttpContext.Items[UserClaim.Session] as string);
            await employeeContext.TerminateSessionAsync(session);
            return Ok();
        }
    }
}
