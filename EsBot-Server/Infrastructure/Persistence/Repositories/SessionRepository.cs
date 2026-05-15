using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;

    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuizRequest> AddSession(QuizRequest quizRequest)
    {
        await _context.QuizRequests.AddAsync(quizRequest);
        await _context.SaveChangesAsync();
        return quizRequest;
    }
}
