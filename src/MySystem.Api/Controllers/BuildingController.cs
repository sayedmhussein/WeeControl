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
    public class BuildingController : Controller
    {
        private readonly ILogger<BuildingController> logger;
        private readonly DataContext context;

        public BuildingController(ILogger<BuildingController> logger, DataContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<BuildingV1Dto>>> GetBuildingV1(Guid id)
        {
            var response = await new BuildingV1Dto(context).GetAsync(id);
            return response != null ? Ok(new ResponseV1<BuildingV1Dto>(response)) : NotFound();
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<BuildingV1Dto>>> GetAllBuildingsV1()
        {
            var response = await new BuildingV1Dto(context).GetAllAsync();
            return response != null ? Ok(new ResponseV1<IEnumerable<BuildingV1Dto>>(response)) : NotFound();
        }

        //[HttpPut]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[Produces(MediaTypeNames.Application.Json)]
        //public async Task<ActionResult<BuildingV1Dto>> PutBuildingV1([FromBody] BuildingV1Dto building)
        //{
        //    return Ok(await BuildingV1Dto.AddOrUpdateAsync(context, building));
        //}

        //[HttpPatch]
        //[Consumes(MediaTypeNames.Application.Json)]
        //public async Task<ActionResult<BuildingV1Dto>> PatchBuildingV1([FromBody] BuildingV1Dto building)
        //{
        //    return Ok(await BuildingV1Dto.AddOrUpdateAsync(context, building, true));
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBuildingsV1(Guid id)
        {
            var response = await new BuildingV1Dto(context).DeleteAsync(id);
            return response ? Ok() : NotFound();
        }
    }
}
