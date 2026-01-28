using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class GenderMapping : Profile
{
    public GenderMapping()
    {
        CreateMap<Gender, GenderResponse>();
    }
}
