using Xunit;
using Moq;
using AutoMapper;
using Core.Services;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Data.Entities;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Exceptions;

namespace Core.Tests.Services;

public class ChatServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ISessionRepository> _mockSessionRepo;
    private readonly Mock<IMessageRepository> _mockMessageRepo;
    private readonly Mock<ILlmInterface> _mockLlm;
    private readonly Mock<IQuizRepository> _mockQuizRepo;
    private readonly ChatService _chatService;

    public ChatServiceTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockSessionRepo = new Mock<ISessionRepository>();
        _mockMessageRepo = new Mock<IMessageRepository>();
        _mockLlm = new Mock<ILlmInterface>();
        _mockQuizRepo = new Mock<IQuizRepository>();

        // System Under Test (SUT)
        _chatService = new ChatService(
            _mockMapper.Object,
            _mockSessionRepo.Object,
            _mockMessageRepo.Object,
            _mockLlm.Object,
            _mockQuizRepo.Object
        );
    }

    [Fact]
    public async Task CreateNewSession_ShouldSaveAndReturnSession()
    {
        // Arrange
        var userId = "user-123";

        // Act
        var result = await _chatService.CreateNewSession(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.ExternalUserId);

        // Verify Interaction with dependency
        _mockSessionRepo.Verify(r => r.AddSession(It.Is<UserSession>(s => s.ExternalUserId == userId)), Times.Once);
    }

    [Fact]
    public async Task AskQuestion_Success_ShouldStoreMessageAndResponse()
    {
        // Arrange
        var request = new QuestionRequest { Content = "What is Mocking?", UserSessionId = Guid.NewGuid() };
        var messageEntity = new Message { Content = "What is Mocking?", UserSessionId = request.UserSessionId };
        var llmStringResponse = "Mocking replaces dependencies.";
        var mappedLlmMessage = new Message { Content = llmStringResponse, UserSessionId = request.UserSessionId };
        var finalResponse = new MessageResponse { Content = llmStringResponse };

        _mockMapper.Setup(m => m.Map<Message>(request)).Returns(messageEntity);
        _mockLlm.Setup(l => l.Ask(messageEntity.Content)).ReturnsAsync(llmStringResponse);
        _mockMapper.Setup(m => m.Map<Message>(llmStringResponse)).Returns(mappedLlmMessage);
        _mockMapper.Setup(m => m.Map<MessageResponse>(mappedLlmMessage)).Returns(finalResponse);

        // Act
        var result = await _chatService.AskQuestion(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(llmStringResponse, result.Content);

        // Verify Interactions: checked that both user prompt and LLM answer were persisted
        _mockMessageRepo.Verify(r => r.AddMessage(messageEntity), Times.Once);
        _mockMessageRepo.Verify(r => r.AddMessage(mappedLlmMessage), Times.Once);
    }

    [Fact]
    public async Task AskQuestion_LLMFailure_ShouldHandleGracefullyAndReturnFallback()
    {
        // Arrange
        var request = new QuestionRequest { Content = "Break Me", UserSessionId = Guid.NewGuid() };
        var messageEntity = new Message { Content = "Break Me", UserSessionId = request.UserSessionId };
        var finalResponse = new MessageResponse { Content = "The AI service is currently unavailable. Please try again later." };

        _mockMapper.Setup(m => m.Map<Message>(request)).Returns(messageEntity);

        // Stub the LLM to throw an exception mimicking failure
        _mockLlm.Setup(l => l.Ask(It.IsAny<string>())).ThrowsAsync(new Exception("API Timeout"));

        _mockMapper.Setup(m => m.Map<MessageResponse>(It.Is<Message>(msg => msg.Content.Contains("unavailable"))))
            .Returns(finalResponse);

        // Act
        var result = await _chatService.AskQuestion(request);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("unavailable", result.Content);
    }

    [Fact]
    public async Task RequestQuiz_Success_ShouldGenerateAndSaveQuiz()
    {
        // Arrange
        var requestDto = new CreateQuizRequest { Topic = "Design Patterns" };
        var quizRequestEntity = new QuizRequest { Topic = "Design Patterns" };
        var mockLlmQuizResult = new QuizResponse { Items = new List<QuizItem> { new QuizItem { Question = "Define Mocking" } } };
        var expectedResponse = new QuizRequestResponse { Topic = "Design Patterns" };

        _mockMapper.Setup(m => m.Map<QuizRequest>(requestDto)).Returns(quizRequestEntity);
        _mockLlm.Setup(l => l.CreateQuiz(quizRequestEntity)).ReturnsAsync(mockLlmQuizResult);
        _mockMapper.Setup(m => m.Map<QuizRequestResponse>(quizRequestEntity)).Returns(expectedResponse);

        // Act
        var result = await _chatService.RequestQuiz(requestDto);

        // Assert
        Assert.NotNull(result);
        _mockQuizRepo.Verify(r => r.AddQuizRequest(quizRequestEntity), Times.Once);
    }

    [Fact]
    public async Task EvaluateAnswer_ShouldReturnFeedback()
    {
        // Arrange
        var request = new AnswerEvaluationRequest { QuestionText = "1+1?", UserAnswer = "2" };
        var expectedFeedback = "Correct response!";
        _mockLlm.Setup(l => l.Evaluate(request.QuestionText, request.UserAnswer)).ReturnsAsync(expectedFeedback);

        // Act
        var result = await _chatService.EvaluateAnswer(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCorrect);
        Assert.Equal(expectedFeedback, result.Feedback);
    }
}
