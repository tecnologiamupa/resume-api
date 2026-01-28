using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class SportsResumeCreateRequestMapping : Profile
{
    public SportsResumeCreateRequestMapping()
    {
        CreateMap<SportsResumeCreateRequest, SportsResume>()
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.SportsSummary, opt => opt.MapFrom(src => src.SportsSummary));

        CreateMap<SportsResumeCreateRequest, SportsResumeUpdateRequest>()
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.SportsSummary, opt => opt.MapFrom(src => src.SportsSummary));
    }
}