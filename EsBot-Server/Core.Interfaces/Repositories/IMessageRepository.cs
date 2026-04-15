using Core.Data.Entities;

namespace Core.Interfaces.Repositories;

public interface IMessageRepository
{
    Task AddMessage(Message message);
    Task<Message?> GetByIdAsync(Guid id);
    Task<IEnumerable<Message>>  GetAll();
}