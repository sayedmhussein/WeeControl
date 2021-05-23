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
using MySystem.Web.EntityFramework;
using MySystem.Web.Shared.Dtos.V1;
using MySystem.Web.Shared.Dtos.V1.Custom;
using MySystem.Web.Api.Security.Policy;
using MySystem.Web.Api.Domain.User;

namespace MySystem.Web.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CredentialsController : Controller
    {
        private readonly IUserContext userContext;
        private readonly ILogger<CredentialsController> logger;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        public CredentialsController(IUserContext userContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<CredentialsController> logger)
        {
            this.logger = logger;
            this.userContext = userContext;
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
            logger?.LogTrace("Function {0} was called", nameof(LoginV1));

            var validation = new ValidationService<LoginDto>(requestDto.Payload);
            if (validation.IsValid == false)
            { 
                return BadRequest(validation.ErrorMessage);
            }

            var session = await userContext.GetUserSessionAsync(requestDto.Payload.Username, requestDto.Payload.Password, requestDto.DeviceId);
            if (session == null)
            {
                logger?.LogInformation("Invalid Attempt to login with invalid username={0}, password={1}.", requestDto.Payload.Username, requestDto.Payload.Password);
                return NotFound();
            }

            var claims = await userContext.GetUserClaims((Guid)session);
            if (claims == null)
            {
                logger?.LogDebug("User has session while claims were null!");
                return Unauthorized();
            }

            var token = new JwtService(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddSeconds(5));
            return Ok(new ResponseDto<string>(token));
        }

        [Authorize(Policy = HasActiveCredentials.Name)]
        [HttpPost("token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ResponseDto<string>>> RefreshTokenV1([FromBody] RequestDto<object> requestDto)
        {
            //var bla = httpContext.HttpContext.Session;
            var session = Guid.Parse(httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == Policies.Claims.Session.Name)?.Value?? Guid.Empty.ToString());
            if (await userContext.SessionIsValidAsync(session) == false)
            {
                return Unauthorized();
            }

            var claims = await userContext.GetUserClaims((Guid)session);
            if (claims == null)
            {
                return Unauthorized();
            }

            var token = new JwtService(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddSeconds(5));
            return Ok(new ResponseDto<string>(token));
        }

        [Authorize(Policy = HasValidSessionAndAccount.Name)]
        [HttpPost("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> LogoutV1([FromBody] RequestDto<object> requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
