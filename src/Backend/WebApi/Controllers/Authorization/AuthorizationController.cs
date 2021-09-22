using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Backend.Application.Common.Interfaces;

namespace WeeControl.Backend.WebApi.Controllers.Authorization
{
    [Route("Api/[controller]")]
    [ApiController]
    public partial class AuthorizationController : Controller
    {
        private readonly IMediator mediatR;
        private readonly ICurrentUserInfo currentUserInfo;

        public AuthorizationController(IMediator mediatR, ICurrentUserInfo currentUserInfo)
        {
            this.mediatR = mediatR;
            this.currentUserInfo = currentUserInfo;
        }
    }
}