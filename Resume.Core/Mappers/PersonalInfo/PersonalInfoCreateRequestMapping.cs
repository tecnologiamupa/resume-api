using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class PersonalInfoCreateRequestMapping : Profile
{
    public PersonalInfoCreateRequestMapping()
    {
        CreateMap<PersonalInfoCreateRequest, PersonalInfo>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId))
            .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.TownshipId, opt => opt.MapFrom(src => src.TownshipId))
            .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.GenderId))
            .ForMember(dest => dest.HasDisability, opt => opt.MapFrom(src => src.HasDisability))
            .ForMember(dest => dest.DisabilityTypeId, opt => opt.MapFrom(src => src.DisabilityTypeId))
            .ForMember(dest => dest.DisabilityDescription, opt => opt.MapFrom(src => src.DisabilityDescription))
            ;

        CreateMap<PersonalInfoCreateRequest, PersonalInfoUpdateRequest>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId))
            .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.TownshipId, opt => opt.MapFrom(src => src.TownshipId))
            .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.GenderId))
            .ForMember(dest => dest.HasDisability, opt => opt.MapFrom(src => src.HasDisability))
            .ForMember(dest => dest.DisabilityTypeId, opt => opt.MapFrom(src => src.DisabilityTypeId))
            .ForMember(dest => dest.DisabilityDescription, opt => opt.MapFrom(src => src.DisabilityDescription))
            ;
    }
}
