using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Web.EntityFramework;
using MySystem.Web.EntityFramework.Models.Basic;
using MySystem.Web.Shared.Dbos;
using MySystem.Web.Shared.Dtos.V1.Entities;

namespace MySystem.Web.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BuildingController : BaseController<BuildingDto, BuildingDbo>
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
