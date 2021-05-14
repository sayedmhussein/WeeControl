using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sayed.MySystem.EntityFramework;
using Sayed.MySystem.EntityFramework.Models.Basic;
using Sayed.MySystem.Shared.Dbos;
using Sayed.MySystem.Shared.Dtos.V1.Entities;

namespace Sayed.MySystem.Api.Controllers.V1
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
