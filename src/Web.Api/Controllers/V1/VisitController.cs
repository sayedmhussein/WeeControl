using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MySystem.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/[controller]")]
    public class VisitController : Controller
    {
        private readonly ILogger<VisitController> logger;

        public VisitController(ILogger<VisitController> logger)
        {
            this.logger = logger;
        }
    }
}
