using Core.Data.Entities;

namespace Core.Interfaces.Repositories;

public interface IQuestionRepository
{
    public Task AddMessage(Message message);

    public Task<IEnumerable<Message>> GetBySessionIdAsync(Guid id);
}