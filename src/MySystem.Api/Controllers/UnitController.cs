using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos;
using MySystem.Data.Data;

namespace MySystem.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UnitController : Controller
    {
        private readonly ILogger<UnitController> logger;

        public UnitController(ILogger<UnitController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitV1Dto>>> GetUnitsV1()
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}
