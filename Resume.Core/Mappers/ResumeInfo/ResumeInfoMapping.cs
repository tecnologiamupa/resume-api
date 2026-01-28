using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class ResumeInfoMapping : Profile
{
    public ResumeInfoMapping()
    {
        CreateMap<ResumeInfo, ResumeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LinkedIn, opt => opt.MapFrom(src => src.LinkedIn))
            .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.PortfolioUrl))
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            ;

        CreateMap<ResumeInfo, ResumeDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LinkedIn, opt => opt.MapFrom(src => src.LinkedIn))
            .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.PortfolioUrl))
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            ;
    }
}
