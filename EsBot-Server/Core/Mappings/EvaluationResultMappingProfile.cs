using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.Entities;

namespace Core.Mappings;

public class EvaluationResultMappingProfile : Profile
{
    public EvaluationResultMappingProfile()
    {
        CreateMap<EvaluationResult, EvaluationResult>();
    }
}