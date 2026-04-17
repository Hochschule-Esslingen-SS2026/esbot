using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services;

public class QuestionManagementService(
    IMapper mapper,
    IQuestionRepository questionRepository,
    ILlmInterface llmInterface)
    : IQuestionManagementService
{

    public async Task<MessageResponse> AskQuestion(QuestionRequest question)
    {
        var message = mapper.Map<Message>(question);
        await questionRepository.AddMessage(message);
        var answer = await llmInterface.Ask(message.Content);

        var answerMessage = mapper.Map<Message>(answer);
        answerMessage.UserSessionId = message.UserSessionId;
        await questionRepository.AddMessage(answerMessage);
        
        return mapper.Map<MessageResponse>(answerMessage);
    }

    public Task<IEnumerable<MessageResponse>> GetQuestionsBySessionId(Guid seesionId)
    {
        throw new NotImplementedException();
    }
}