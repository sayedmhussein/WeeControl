using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Backend.WebApi.Security.Policies;

namespace WeeControl.Backend.WebApi.Controllers.Territory
{
    [Route("Api/[controller]")]
    [Authorize(Policy = BasicPolicies.HasActiveSession)]
    [ApiController]
    public partial class TerritoryController : Controller
    {
        private readonly IMediator mediatR;

        public TerritoryController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }
    }
}
