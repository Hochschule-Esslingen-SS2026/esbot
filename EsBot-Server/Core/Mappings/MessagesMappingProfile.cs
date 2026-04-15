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
        CreateMap<CreateMessageRequest, Message>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src =>DateTime.UtcNow));
        CreateMap<IEnumerable<Message>, AllMessagesResponse>()
            .ForMember(dest => dest.AllMessages, 
                opt => opt.MapFrom(src => src));
        
    }
}