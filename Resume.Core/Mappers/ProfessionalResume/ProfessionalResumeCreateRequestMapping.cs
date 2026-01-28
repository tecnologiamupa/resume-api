using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class ProfessionalResumeCreateRequestMapping : Profile
{
    public ProfessionalResumeCreateRequestMapping()
    {
        CreateMap<ProfessionalResumeCreateRequest, ProfessionalResume>()
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.ProfessionalSummary, opt => opt.MapFrom(src => src.ProfessionalSummary));

        CreateMap<ProfessionalResumeCreateRequest, ProfessionalResumeUpdateRequest>()
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.ProfessionalSummary, opt => opt.MapFrom(src => src.ProfessionalSummary));
    }
}