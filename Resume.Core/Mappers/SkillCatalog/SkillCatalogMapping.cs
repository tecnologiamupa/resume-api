using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class SkillCatalogMapping : Profile
{
    public SkillCatalogMapping()
    {
        CreateMap<SkillCatalog, SkillCatalogResponse>();
    }
}
