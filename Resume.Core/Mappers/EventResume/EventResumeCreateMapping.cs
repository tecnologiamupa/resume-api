using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class EventResumeCreateMapping : Profile
{
    public EventResumeCreateMapping()
    {
        CreateMap<EventResumeCreateRequest, EventResume>();
    }
}
