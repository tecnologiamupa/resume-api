using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class InternshipResumeMapping : Profile
{
    public InternshipResumeMapping()
    {
        CreateMap<InternshipResume, InternshipResumeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.CareerObjective, opt => opt.MapFrom(src => src.CareerObjective));
    }
}