using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class LanguageCatalogMapping : Profile
{
    public LanguageCatalogMapping()
    {
        CreateMap<LanguageCatalog, LanguageCatalogResponse>();
    }
}
