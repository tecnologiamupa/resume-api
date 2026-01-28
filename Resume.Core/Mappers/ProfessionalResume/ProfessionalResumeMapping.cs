using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class ProfessionalResumeMapping : Profile
{
    public ProfessionalResumeMapping()
    {
        CreateMap<ProfessionalResume, ProfessionalResumeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.ResumeId))
            .ForMember(dest => dest.ProfessionalSummary, opt => opt.MapFrom(src => src.ProfessionalSummary))
            //.ForMember(dest => dest.IsInternshipCandidate, opt => opt.MapFrom(src => src.IsInternshipCandidate))
            //.ForMember(dest => dest.InternshipTypeId, opt => opt.MapFrom(src => src.InternshipTypeId))
            //.ForMember(dest => dest.IsInadehCandidate, opt => opt.MapFrom(src => src.IsInadehCandidate))
            //.ForMember(dest => dest.InadehCourseId, opt => opt.MapFrom(src => src.InadehCourseId))
            ;
    }
}