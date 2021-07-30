using System;
using AutoMapper;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Extensions
{
    public static class EntityMappingExtension
    {
        public static TDbo ToDbo<TDto, TDbo>(this IAggregateRoot dto)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<TDto, TDbo>().ReverseMap();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<TDbo>(dto);
        }

        public static TDto ToDto<TDbo, TDto>(this IAggregateRoot dbo)
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
