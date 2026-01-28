using AutoMapper;
using Resume.Core.DTOs;

namespace Resume.Core.Mappers;

public class ResumeWithVacancyMapping : Profile
{
    public ResumeWithVacancyMapping()
    {
        CreateMap<ResumeWithVacancy, ResumeWithVacancyResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LinkedIn, opt => opt.MapFrom(src => src.LinkedIn))
            .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.PortfolioUrl))
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.VacancyName, opt => opt.MapFrom(src => src.VacancyName))
            ;
    }
}
