using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos;
using MySystem.Api.Dtos.V1;
using MySystem.Data;
using MySystem.Data.Models.Basic;

namespace MySystem.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BuildingController : BaseController<BuildingDto, BuildingDto, Building>
    {
        private readonly ILogger<BuildingController> logger;
        private readonly DataContext context;

        public BuildingController(ILogger<BuildingController> logger, DataContext context) : base(logger, new BuildingDto(context))
        {
            this.logger = logger;
            this.context = context;
        }
    }
}
