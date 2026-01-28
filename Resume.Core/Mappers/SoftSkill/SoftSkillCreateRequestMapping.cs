using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class SoftSkillCreateRequestMapping : Profile
{
    public SoftSkillCreateRequestMapping()
    {
        CreateMap<SoftSkillCreateRequest, SoftSkill>()
            .ForMember(dest => dest.ProfessionalResumeId, opt => opt.MapFrom(src => src.ProfessionalResumeId))
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill));
    }
}