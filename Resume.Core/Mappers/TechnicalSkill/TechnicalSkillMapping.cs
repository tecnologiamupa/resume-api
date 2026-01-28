using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class TechnicalSkillMapping : Profile
{
    public TechnicalSkillMapping()
    {
        CreateMap<TechnicalSkill, TechnicalSkillResponse>()
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill));
    }
}