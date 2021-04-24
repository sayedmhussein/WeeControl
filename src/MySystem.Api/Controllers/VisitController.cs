using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Data.Data;

namespace MySystem.Api.Controllers
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
