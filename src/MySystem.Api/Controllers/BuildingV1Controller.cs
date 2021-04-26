using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos;
using MySystem.Data.Data;
using MySystem.Data.Models.Basic;
using MySystem.Data.V1;
using MySystem.Data.V1.Dtos;

namespace MySystem.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BuildingController : BaseV1Controller<BuildingV1Dto, BuildingV1Dto, Building>
    {
        private readonly ILogger<BuildingController> logger;
        private readonly DataContext context;

        public BuildingController(ILogger<BuildingController> logger, DataContext context) : base(logger, new BuildingV1Dto(context))
        {
            this.logger = logger;
            this.context = context;
        }
    }
}
