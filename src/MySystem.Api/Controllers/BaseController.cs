using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Data.Data;
using MySystem.Data.V1;

namespace MySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TDto, TRepository> : ControllerBase
        where TEntity : class
        where TDto : class
        where TRepository : Repository<TEntity, TDto>
    {
        private readonly ILogger<BaseController<TEntity, TDto, TRepository>> logger;
        private readonly TRepository repository;

        public BaseController(ILogger<BaseController<TEntity, TDto, TRepository>> logger, TRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            return Ok(await repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
