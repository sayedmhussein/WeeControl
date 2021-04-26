using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos;
using MySystem.Api.Helpers;
using MySystem.Data.Data;
using MySystem.Data.V1.Dtos;

namespace MySystem.Api.Controllers
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
        public async Task<ActionResult<ResponseV1Dto<string>>> LoginV1([FromBody] RequestV1Dto<LoginV1Dto> requestDto)
        {
            var session = await requestDto.Payload.GetSessionAsync(context);
            if (session != null)
            {
                var claims = await LoginV1Dto.GetUserClaimsAsync(context, (Guid)session);
                var token = new JwtOperation(configuration["Jwt:Key"]).GenerateJwtToken(claims, configuration["Jwt:Issuer"], DateTime.UtcNow.AddMinutes(5));
                return Ok(new ResponseV1Dto<string>(token));
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
        public async Task<ActionResult<ResponseV1Dto<string>>> RefreshTokenV1([FromBody] RequestV1Dto<object> requestDto)
        {
            throw new NotImplementedException();
        }

        //[Authorize(Policy = Policies.HasActiveSession)]
        //[Authorize(Policy = Policies.HasRefreshedSession)]
        [HttpPost("logout")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> LogoutV1([FromBody] RequestV1Dto<object> requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
