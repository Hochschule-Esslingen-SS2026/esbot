using Core.Data.Entities;

namespace Core.Interfaces.Repositories;

public interface ISessionRepository
{
    Task AddSession(UserSession session);
    Task<UserSession?> GetSession(Guid sessionId);
    Task<IEnumerable<UserSession>> GetSessionsByUserId(string externalUserId);
    Task AddMessage(Message message);
    Task<IEnumerable<Message>> GetMessagesBySessionIdAsync(Guid id);
    Task UpdateSession(UserSession session);
    Task DeleteSession(Guid sessionId);
    Task<QuizRequest> AddQuizRequest(QuizRequest quizRequest);
}
