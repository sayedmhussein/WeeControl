using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos.V1;
using MySystem.Api.Helpers;
using MySystem.Api.Policies;
using MySystem.Data;

namespace MySystem.Api.Controllers.V1
{
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CredentialsController : Controller
    {
        private readonly ILogger<CredentialsController> logger;
        private readonly DataContext context;
        private readonly IConfiguration configuration;

        public CredentialsController(ILogger<CredentialsController> logger, DataContext context, IConfiguration configuration)
        {
            this.logger = logger;
            this.context = context;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ResponseDto<string>>> LoginV1([FromBody] RequestDto<LoginDto> requestDto)
        {
            var session = await new CustomFunctionV1(context).GetSessionAsync(requestDto.Payload.Username, requestDto.Payload.Password, requestDto.DeviceId);
            if (session == null)
            {
                return NotFound();
                
            }

            var claims = await new CustomFunctionV1(context).GetUserClaimsAsync(context, (Guid)session);
            if (claims == null)
            {
                return Unauthorized();
            }

            var token = new JwtOperation(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddMinutes(5));
            return Ok(new ResponseDto<string>(token));

        }

        [Authorize(Policy = HasRefreshedSession.Name)]
        [HttpPost("token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ResponseDto<string>>> RefreshTokenV1([FromBody] RequestDto<object> requestDto)
        {
            var session = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == Policies.Claims.Session.Name)?.Value?? Guid.Empty.ToString());

            var claims = await new CustomFunctionV1(context).GetUserClaimsAsync(context, session);
            if (claims == null)
            {
                return Unauthorized();
            }

            var token = new JwtOperation(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddMinutes(5));
            return Ok(new ResponseDto<string>(token));
        }

        //[Authorize(Policy = Policies.HasActiveSession)]
        //[Authorize(Policy = Policies.HasRefreshedSession)]
        [HttpPost("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> LogoutV1([FromBody] RequestDto<object> requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
