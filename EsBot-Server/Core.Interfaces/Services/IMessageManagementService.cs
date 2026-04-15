using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;

namespace Core.Interfaces.Services;

public interface IMessageManagementService
{
    Task<MessageResponse> CreatePersonAsync(CreateMessageRequest messageRequest);
    Task<MessageResponse> GetByIdAsync(Guid id);
    Task<AllMessagesResponse> GetAllAsync();
}