using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;

    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddSession(UserSession session)
    {
        await _context.UserSessions.AddAsync(session);
        await _context.SaveChangesAsync();
    }

    public async Task<UserSession?> GetSession(Guid sessionId)
    {
        // Eagerly load messages to construct full session object graph
        return await _context.UserSessions
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }

    public async Task<IEnumerable<UserSession>> GetSessionsByUserId(string externalUserId)
    {
        return await _context.UserSessions
            .Where(s => s.ExternalUserId == externalUserId)
            .ToListAsync();
    }

    public async Task AddMessage(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesBySessionIdAsync(Guid id)
    {
        // Ensures chronological ordering by creation timestamp or Id
        return await _context.Messages
            .Where(m => m.UserSessionId == id)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateSession(UserSession session)
    {
        _context.UserSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSession(Guid sessionId)
    {
        // Use FirstOrDefaultAsync with Include to load the session and all its relations
        var session = await _context.UserSessions
            .Include(s => s.Messages)
            .Include(s => s.QuizRequests)
            .ThenInclude(q => q.QuizItems) // Needed to load nested QuizItems
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session != null)
        {
            // 1. Delete deeply nested items first
            _context.QuizItems.RemoveRange(session.QuizRequests.SelectMany(s => s.QuizItems));

            // 2. Delete direct child dependents
            _context.Messages.RemoveRange(session.Messages);
            _context.QuizRequests.RemoveRange(session.QuizRequests);

            // 3. Delete the parent session
            _context.UserSessions.Remove(session);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<QuizRequest> AddQuizRequest(QuizRequest quizRequest)
    {
        await _context.QuizRequests.AddAsync(quizRequest);
        if (quizRequest.QuizItems != null && quizRequest.QuizItems.Any())
        {
            await _context.QuizItems.AddRangeAsync(quizRequest.QuizItems);
        }
        await _context.SaveChangesAsync();
        return quizRequest;
    }
}
