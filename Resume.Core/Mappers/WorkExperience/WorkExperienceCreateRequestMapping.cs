using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class WorkExperienceCreateRequestMapping : Profile
{
    public WorkExperienceCreateRequestMapping()
    {
        CreateMap<WorkExperienceCreateRequest, WorkExperience>()
            .ForMember(dest => dest.ProfessionalResumeId, opt => opt.MapFrom(src => src.ProfessionalResumeId))
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.CurrentlyWorking, opt => opt.MapFrom(src => src.CurrentlyWorking))
            .ForMember(dest => dest.PositionDescription, opt => opt.MapFrom(src => src.PositionDescription))
            ;
    }
}