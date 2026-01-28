using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class SoftSkillMapping : Profile
{
    public SoftSkillMapping()
    {
        CreateMap<SoftSkill, SoftSkillResponse>()
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill));
    }
}