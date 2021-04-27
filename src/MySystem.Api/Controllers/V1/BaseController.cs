using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos.V1;
using MySystem.Data;

namespace MySystem.Api.Controllers.V1
{
    //[Authorize(Policy = "Active")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public abstract class BaseController<TRepository, TDto, TEntity> : ControllerBase
        where TEntity : class
        where TDto : class
        where TRepository : RepositoryV1<TDto, TEntity>
    {
        private readonly ILogger logger;
        private readonly TRepository repository;

        public BaseController(ILogger logger, TRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var response = await repository.GetAllAsync();
            return response == null ? NotFound() : Ok(new ResponseDto<IEnumerable<TDto>>(response));
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> Get(Guid id)
        {
            var response = await repository.GetAsync(id);
            return response == null ? NotFound() : Ok(new ResponseDto<TDto>(response));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> PutBuildingV1([FromBody] RequestDto<TDto> request)
        {
            var response = await repository.AddOrUpdateAsync(request.Payload);
            return response != null ? Ok(new ResponseDto<TDto>(response)) : BadRequest();
        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> PatchBuildingV1([FromBody] RequestDto<TDto> request)
        {
            var response = await repository.PatchAsync(request.Payload);
            return response != null ? Ok(new ResponseDto<TDto>(response)) : BadRequest();
        }

        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteBuildingsV1(Guid id)
        {
            var response = await repository.DeleteAsync(id);
            return response ? Ok(new ResponseDto<bool>(response)) : NotFound();
        }
    }
}
