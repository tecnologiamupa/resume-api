using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class LanguageCreateRequestMapping : Profile
{
    public LanguageCreateRequestMapping()
    {
        CreateMap<LanguageCreateRequest, PersonalLanguage>()
            //.ForMember(dest => dest.PersonalInfoId, opt => opt.MapFrom(src => src.PersonalInfoId))
            .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}