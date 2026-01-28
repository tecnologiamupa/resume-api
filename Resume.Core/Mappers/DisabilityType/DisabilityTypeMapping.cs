using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class DisabilityTypeMapping : Profile
{
    public DisabilityTypeMapping()
    {
        CreateMap<DisabilityType, DisabilityTypeResponse>();
    }
}
