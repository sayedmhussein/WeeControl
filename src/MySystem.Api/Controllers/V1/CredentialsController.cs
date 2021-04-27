using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos.V1;
using MySystem.Api.Helpers;
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
            var session = await requestDto.Payload.GetSessionAsync(context);
            if (session != null)
            {
                var claims = await LoginDto.GetUserClaimsAsync(context, (Guid)session);
                var token = new JwtOperation(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddMinutes(5));
                return Ok(new ResponseDto<string>(token));
            }
            else
            {
                return NotFound();
            }
        }

        //[Authorize(Policy = Policies.HasActiveSession)]
        [HttpPost("token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ResponseDto<string>>> RefreshTokenV1([FromBody] RequestDto<object> requestDto)
        {
            throw new NotImplementedException();
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
