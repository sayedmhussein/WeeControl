using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.WebApi.Controllers.Authorization
{
    [Route("Api/[controller]")]
    [ApiController]
    public partial class AuthorizationController : Controller
    {
        private readonly IMediator mediatR;
        private readonly IJwtService jwtService;

        public AuthorizationController(IMediator mediatR, IJwtService jwtService)
        {
            this.mediatR = mediatR;
            this.jwtService = jwtService;
        }
    }
}