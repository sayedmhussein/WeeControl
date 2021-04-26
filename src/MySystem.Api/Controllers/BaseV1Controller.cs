using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos;
using MySystem.Data.Data;
using MySystem.Data.V1;
using MySystem.Data.V1.Dtos;

namespace MySystem.Api.Controllers
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public abstract class BaseV1Controller<TRepository, TDto, TEntity> : ControllerBase
        where TEntity : class
        where TDto : class
        where TRepository : RepositoryV1<TDto, TEntity>
    {
        private readonly ILogger logger;
        private readonly TRepository repository;

        public BaseV1Controller(ILogger logger, TRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var response = await repository.GetAllAsync();
            return response == null ? NotFound() : Ok(new ResponseV1Dto<IEnumerable<TDto>>(response));
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TDto>> Get(Guid id)
        {
            var response = await repository.GetAsync(id);
            return response == null ? NotFound() : Ok(new ResponseV1Dto<TDto>(response));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TDto>> PutBuildingV1([FromBody] RequestV1Dto<TDto> request)
        {
            var response = await repository.AddOrUpdateAsync(request.Payload);
            return Ok(new ResponseV1Dto<TDto>(response));
        }

        //[HttpPatch]
        //[Consumes(MediaTypeNames.Application.Json)]
        //public async Task<ActionResult<TDto>> PatchBuildingV1([FromBody] RequestV1Dto<TDto> request)
        //{
        //    return Ok(await repository.AddOrUpdateAsync(request.Payload));
        //}

        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> DeleteBuildingsV1(Guid id)
        {
            var response = await repository.DeleteAsync(id);
            return response ? Ok(new ResponseV1Dto<bool>(response)) : NotFound();
        }
    }
}
