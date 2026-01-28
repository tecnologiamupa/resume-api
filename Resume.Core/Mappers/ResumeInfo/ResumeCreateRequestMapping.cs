using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class ResumeCreateRequestMapping : Profile
{
    public ResumeCreateRequestMapping()
    {
        CreateMap<ResumeCreateRequest, ResumeInfo>()
            .ForMember(dest => dest.ResumeTypeId, opt => opt.MapFrom(src => src.ResumeTypeId))
            .ForMember(dest => dest.LinkedIn, opt => opt.MapFrom(src => src.LinkedIn))
            .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.PortfolioUrl))
            ;
    }
}
