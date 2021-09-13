using AutoMapper;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Extensions
{
    public static class EntityMappingExtension
    {
        public static TDbo ToDbo<TDto, TDbo>(this ISerializable dto)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TDto, TDbo>().ReverseMap();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<TDbo>(dto);
        }

        public static TDto ToDto<TDbo, TDto>(this ISerializable dbo)
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
