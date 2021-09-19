using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeeControl.Backend.WebApi.Controllers.Territory
{
    [Route("Api/[controller]")]
    [Authorize]
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
