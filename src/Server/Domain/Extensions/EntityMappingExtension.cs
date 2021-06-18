using AutoMapper;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.Server.Domain.Extensions
{
    public static class EntityMappingExtension
    {
        public static TDbo ToDbo<TDto, TDbo>(this IEntityDto dto)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TDto, TDbo>().ReverseMap();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<TDbo>(dto);
        }

        public static TDto ToDto<TDbo, TDto>(this IEntityDbo dbo)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TDbo, TDto>().ReverseMap();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<TDto>(dbo);
        }
    }
}
