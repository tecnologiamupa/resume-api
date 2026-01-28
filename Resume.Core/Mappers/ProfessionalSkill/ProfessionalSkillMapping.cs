using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class ProfessionalSkillMapping : Profile
{
    public ProfessionalSkillMapping()
    {
        CreateMap<ProfessionalSkill, ProfessionalSkillResponse>();
    }
}
