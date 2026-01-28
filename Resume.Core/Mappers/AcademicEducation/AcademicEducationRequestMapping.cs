using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class AcademicEducationRequestMapping : Profile
{
    public AcademicEducationRequestMapping()
    {
        CreateMap<AcademicEducationRequest, AcademicEducationCreateRequest>()
            .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.Institution))
            .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.Degree))
            .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.FieldOfStudy))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.CurrentlyStudying, opt => opt.MapFrom(src => src.CurrentlyStudying))
            .ForMember(dest => dest.AdditionalDescription, opt => opt.MapFrom(src => src.AdditionalDescription));
    }
}