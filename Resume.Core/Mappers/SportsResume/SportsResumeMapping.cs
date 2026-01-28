using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class SportsResumeMapping : Profile
{
    public SportsResumeMapping()
    {
        CreateMap<SportsResume, SportsResumeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.SportsSummary, opt => opt.MapFrom(src => src.SportsSummary));
    }
}