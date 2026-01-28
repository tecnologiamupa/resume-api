using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class InternshipResumeCreateRequestMapping : Profile
{
    public InternshipResumeCreateRequestMapping()
    {
        CreateMap<InternshipResumeCreateRequest, InternshipResume>()
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.CareerObjective, opt => opt.MapFrom(src => src.CareerObjective));

        CreateMap<InternshipResumeCreateRequest, InternshipResumeUpdateRequest>()
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.CareerObjective, opt => opt.MapFrom(src => src.CareerObjective));
    }
}