using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;

namespace Core.Mappings;

public class QuizMappingProfile : Profile
{
    public QuizMappingProfile()
    {
        CreateMap<CreateQuizRequest, QuizRequest>();
        
        CreateMap<QuizItem, QuizItemResponse>();
        
        CreateMap<QuizRequest, QuizRequestResponse>()
            .ForMember(dest => dest.QuizItems, opt => opt.MapFrom(src => src.QuizItems));
    }
}