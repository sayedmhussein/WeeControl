using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Data;
using MySystem.Data.V1.Dtos;

namespace MySystem.Data.V1
{
    public abstract class Repository<TEntity, TDto> where TEntity : class
    {
        protected DataContext context;
        protected IMapper mapper;

        public Repository()
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

        public async Task<TDto> AddAsync(TEntity entity)
        {
            context.Set<TEntity>().Add(mapper.Map<TEntity>(entity));
            await context.SaveChangesAsync();
            return mapper.Map<TDto>(entity);
        }

        public async Task<TDto> UpdateAsync(TEntity entity)
        {
            context.Entry(mapper.Map<TEntity>(entity)).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return mapper.Map<TDto>(entity);
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
