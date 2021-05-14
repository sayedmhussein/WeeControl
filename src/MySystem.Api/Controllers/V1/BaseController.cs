using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sayed.MySystem.Shared.Dtos.V1;

namespace Sayed.MySystem.Api.Controllers.V1
{
    //[Authorize(Policy = "Active")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public abstract class BaseController<TDto, TEntity> : ControllerBase
        where TDto : class
        where TEntity : class
    {
        private readonly ILogger logger;
        private readonly DbContext context;
        private IMapper mapper;

        public BaseController(ILogger logger, DbContext context)
        {
            this.logger = logger;
            this.context = context;

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TEntity, TDto>().ReverseMap();
            });

            mapper = config.CreateMapper();
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var list = new List<TDto>();
            (await context.Set<TEntity>().ToListAsync()).ForEach(x => list.Add(mapper.Map<TDto>(x)));

            if (list.Count == 0)
            {
                return NotFound();
            }

            return Ok(new ResponseDto<IEnumerable<TDto>>(list));
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> Get(Guid id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<TDto>(entity);
            return Ok(new ResponseDto<TDto>(dto));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> PutBuildingV1([FromBody] RequestDto<TDto> request)
        {
            bool id = Guid.TryParse(((dynamic)request.Payload)?.Id?.ToString(), out Guid key);
            if (id)
            {
                var item = await context.Set<TEntity>().FindAsync(key);
                if (item != null)
                {
                    mapper.Map(request.Payload, item);
                    context.Update(item);
                    await context.SaveChangesAsync();
                    return mapper.Map<TDto>(item);
                }
            }

            var item_ = mapper.Map<TEntity>(request.Payload);
            context.Set<TEntity>().Add(item_);
            await context.SaveChangesAsync();

            return await Get(key);
        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> PatchBuildingV1([FromBody] RequestDto<TDto> request)
        {
            bool id = Guid.TryParse(((dynamic)request.Payload)?.Id?.ToString(), out Guid key);
            if (id)
            {
                var item = await context.Set<TEntity>().FindAsync(key);
                if (item != null)
                {
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<TDto, TEntity>()
                        .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal != null));
                    });
                    config.CreateMapper().Map(request.Payload, item);
                    context.Update(item);
                    await context.SaveChangesAsync();
                    return mapper.Map<TDto>(item);
                }
            }

            return await Get(key);
        }

        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TDto>> DeleteBuildingsV1(Guid id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
            }

            return await Get(id);
        }
    }
}
