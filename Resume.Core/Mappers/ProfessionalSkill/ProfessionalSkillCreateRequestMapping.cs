using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class ProfessionalSkillCreateRequestMapping : Profile
{
    public ProfessionalSkillCreateRequestMapping()
    {
        CreateMap<ProfessionalSkillCreateRequest, ProfessionalSkill>();
    }
}
