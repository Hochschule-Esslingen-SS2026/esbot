using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;

namespace Core.Mappings;

public class MessagesMappingProfile : Profile
{
    public MessagesMappingProfile()
    {
        CreateMap<Message, MessageResponse>();
        CreateMap<QuestionRequest, Message>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.UserSessionId, opt => opt.MapFrom(src => src.UserSessionId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => true));
        CreateMap<IEnumerable<Message>, AllMessagesResponse>()
            .ForMember(dest => dest.AllMessages, 
                opt => opt.MapFrom(src => src));
        CreateMap<string, Message>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => false));
        
    }
}