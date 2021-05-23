using System;
using AutoMapper;
using MySystem.Shared.Library.Dbos;
using MySystem.Shared.Library.Dtos;

namespace MySystem.Shared.Library.ExtensionMethods
{
    public static class EntitesConvertionExtension
    {
        public static TDbo ToDbo<TDto, TDbo>(this IDto dto)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TDto, TDbo>().ReverseMap();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<TDbo>(dto);
        }

        public static TDto ToDto<TDbo, TDto>(this IDbo dbo)
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
