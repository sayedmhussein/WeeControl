using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sayed.MySystem.ServerData;
using Sayed.MySystem.ServerData.Models.Basic;
using Sayed.MySystem.SharedDto.V1.Entities;

namespace Sayed.MySystem.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BuildingController : BaseController<BuildingDto, Building>
    {
        private readonly ILogger<BuildingController> logger;
        private readonly DataContext context;

        public BuildingController(ILogger<BuildingController> logger, DataContext context) : base(logger, context)
        {
            this.logger = logger;
            this.context = context;
        }
    }
}
