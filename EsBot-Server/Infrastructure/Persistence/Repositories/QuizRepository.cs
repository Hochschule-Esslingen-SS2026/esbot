using Core.Interfaces.Repositories;
using Core.Data.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly ApplicationDbContext _context;

    public QuizRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuizRequest> AddQuizRequest(QuizRequest quizRequest)
    {
        await _context.QuizRequests.AddAsync(quizRequest);
        await _context.SaveChangesAsync();
        return quizRequest;
    }
}