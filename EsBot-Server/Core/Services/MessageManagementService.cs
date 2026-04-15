using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services;

public class MessageManagementService : IMessageManagementService
{
    private readonly IMapper _mapper;
    private readonly IMessageRepository _messageRepository;

    public MessageManagementService( IMapper mapper, IMessageRepository messageRepository)
    {
        _mapper = mapper;
        _messageRepository = messageRepository;

    }

    public async Task<MessageResponse> CreatePersonAsync(CreateMessageRequest messageRequest)
    {
        var message = _mapper.Map<Message>(messageRequest);
        await _messageRepository.AddMessage(message);
        return _mapper.Map<MessageResponse>(message);
    }

    public async Task<MessageResponse> GetByIdAsync(Guid id)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        return _mapper.Map<MessageResponse>(message);
    }
    public async Task<AllMessagesResponse> GetAllAsync()
    {
        var messages = await _messageRepository.GetAll();
        return _mapper.Map<AllMessagesResponse>(messages);
    }
}