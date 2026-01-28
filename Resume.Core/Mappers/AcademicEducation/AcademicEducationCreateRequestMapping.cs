using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class AcademicEducationCreateRequestMapping : Profile
{
    public AcademicEducationCreateRequestMapping()
    {
        CreateMap<AcademicEducationCreateRequest, AcademicEducation>()
            .ForMember(dest => dest.ProfessionalResumeId, opt => opt.MapFrom(src => src.ProfessionalResumeId))
            .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.Institution))
            .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.Degree))
            .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.FieldOfStudy))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.CurrentlyStudying, opt => opt.MapFrom(src => src.CurrentlyStudying))
            .ForMember(dest => dest.AdditionalDescription, opt => opt.MapFrom(src => src.AdditionalDescription));
    }
}