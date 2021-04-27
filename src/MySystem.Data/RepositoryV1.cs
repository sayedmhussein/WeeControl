using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MySystem.Data
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
                c.CreateMap<TEntity, TDto>().ReverseMap();
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
            bool id = Guid.TryParse(((dynamic)dto)?.Id?.ToString(), out Guid key);
            if (id)
            {
                var item = await context.Set<TEntity>().FindAsync(key);
                if (item != null)
                {
                    mapper.Map(dto, item);
                    context.Update(item);
                    await context.SaveChangesAsync();
                    return mapper.Map<TDto>(item);
                }
            }

            var item_ = mapper.Map<TEntity>(dto);
            context.Set<TEntity>().Add(item_);
            await context.SaveChangesAsync();
            return mapper.Map<TDto>(item_);
        }

        public async Task<TDto> PatchAsync(TDto dto)
        {
            bool id = Guid.TryParse(((dynamic)dto)?.Id?.ToString(), out Guid key);
            if (id)
            {
                var item = await context.Set<TEntity>().FindAsync(key);
                if (item != null)
                {
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<TDto, TEntity>()
                        //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
                        .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal != null));
                    });
                    config.CreateMapper().Map(dto, item);
                    //mapper.Map(dto, item)
                    context.Update(item);
                    await context.SaveChangesAsync();
                    return mapper.Map<TDto>(item);
                }
            }

            return null;
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
