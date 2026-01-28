using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class LanguageMapping : Profile
{
    public LanguageMapping()
    {
        CreateMap<PersonalLanguage, LanguageResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}