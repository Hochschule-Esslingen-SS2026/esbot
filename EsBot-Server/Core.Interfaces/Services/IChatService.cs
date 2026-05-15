using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;

namespace Core.Interfaces.Services;

public interface IChatService
{
    public UserSession CreateNewSession(); //TODO how makes it

    public MessageResponse AskAQuestion(QuestionRequest message);
    public QuizRequest AskAQuizRequest(QuizRequest request);
    public EvaluationResult EvaluateAnswer(QuestionRequest question);


}
