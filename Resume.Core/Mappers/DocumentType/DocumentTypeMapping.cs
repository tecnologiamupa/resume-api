using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.Mappers;

public class DocumentTypeMapping : Profile
{
    public DocumentTypeMapping()
    {
        CreateMap<DocumentType, DocumentTypeResponse>();
    }
}
