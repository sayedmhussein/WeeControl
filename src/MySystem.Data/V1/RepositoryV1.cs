using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Data;
using MySystem.Data.V1.Dtos;

namespace MySystem.Data.V1
{
    public abstract class RepositoryV1<TDto, TEntity>
        where TEntity : class
        where TDto : class
    {
        protected DataContext context;
        protected IMapper mapper;

        public RepositoryV1()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TEntity, TDto>();
                c.CreateMap<TDto, TEntity>();
            });

            mapper = config.CreateMapper();
        }

        public async Task<TDto> GetAsync(Guid id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            return mapper.Map<TDto>(entity); ;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var list = new List<TDto>();
            (await context.Set<TEntity>().ToListAsync()).ForEach(x => list.Add(mapper.Map<TDto>(x)));
            return list;
        }

        public async Task<TDto> AddOrUpdateAsync(TDto dto)
        {
            dynamic _entity = mapper.Map<TEntity>(dto);
            if (await context.Set<TEntity>().FindAsync(_entity.Id) == null)
            {
                context.Set<TEntity>().Add(_entity);
            }
            else
            {
                context.Update(_entity);
                //context.Attach(_entity);
                //context.Entry(_entity).State = EntityState.Modified;
            }
            
            await context.SaveChangesAsync();
            return mapper.Map<TDto>(_entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
