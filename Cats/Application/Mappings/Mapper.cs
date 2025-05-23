﻿using AutoMapper;
using Contracts;
using Entities.Models;

namespace Mappings
{
    public class Mapper : Profile
    {
        public Mapper()
        {

            CreateMap<CatDto, Cat>()
                .ForMember(dest => dest.CatId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Created, opt => opt.Ignore());

            CreateMap<BreedDto, Tag>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Temperament));

            CreateMap<List<BreedDto>, List<Tag>>()
                .ConvertUsing((src, dest, context) =>
                    src.Select(b => context.Mapper.Map<Tag>(b)).ToList());


            CreateMap<Cat, CatDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CatId))
        .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
        .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
        .ForMember(dest => dest.Breeds, opt => opt.MapFrom(src => src.Tags)); ;


            CreateMap<Tag, BreedDto>()
                .ForMember(dest => dest.Temperament, opt => opt.MapFrom(src => src.Name));

            CreateMap<List<Tag>, List<BreedDto>>()
                .ConvertUsing((src, dest, context) =>
                    src.Select(tag => context.Mapper.Map<BreedDto>(tag)).ToList());
        }
    }
}
