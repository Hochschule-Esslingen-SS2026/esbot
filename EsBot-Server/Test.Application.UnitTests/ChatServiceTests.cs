using AutoMapper;
using Core.Data.DTOs;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using FakeItEasy;

namespace Test.Application.UnitTests;

public class ChatServiceTests
{
    private readonly IMapper _fakeMapper;
    private readonly ISessionRepository _fakeSessionRepo;
    private readonly ILlmInterface _fakeLlm;
    private readonly ChatService _chatService;

    public ChatServiceTests()
    {
        // Initializing Fakes using FakeItEasy
        _fakeMapper = A.Fake<IMapper>();
        _fakeSessionRepo = A.Fake<ISessionRepository>();
        _fakeLlm = A.Fake<ILlmInterface>();

        // System Under Test (SUT)
        _chatService = new ChatService(
            _fakeMapper,
            _fakeSessionRepo,
            _fakeLlm
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

        // Asserting interaction using FakeItEasy syntax
        A.CallTo(() => _fakeSessionRepo.AddSession(A<UserSession>.That.Matches(s => s.ExternalUserId == userId)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AskQuestion_Success_ShouldStoreMessageAndResponse()
    {
        // Arrange
        var request = new QuestionRequest { Question = "What is Mocking?", UserSessionId = Guid.NewGuid() };
        var messageEntity = new Message(request.UserSessionId, true, "What is Mocking");
        var llmStringResponse = "Mocking replaces dependencies.";
        var mappedLlmMessage = new Message (request.UserSessionId, true, llmStringResponse);
        var finalResponse = new MessageResponse { Id = Guid.NewGuid(), UserSessionId = request.UserSessionId, Content = llmStringResponse, Role = true , Timestamp =  DateTime.Now };

        // Configuring behavior
        A.CallTo(() => _fakeMapper.Map<Message>(request)).Returns(messageEntity);
        A.CallTo(() => _fakeLlm.Ask(messageEntity.Content)).Returns(llmStringResponse);
        A.CallTo(() => _fakeMapper.Map<Message>(llmStringResponse)).Returns(mappedLlmMessage);
        A.CallTo(() => _fakeMapper.Map<MessageResponse>(mappedLlmMessage)).Returns(finalResponse);

        // Act
        var result = await _chatService.AskQuestion(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(llmStringResponse, result.Content);

        // Verify that both user prompt and LLM answer were persisted
        A.CallTo(() => _fakeSessionRepo.AddMessage(messageEntity)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeSessionRepo.AddMessage(mappedLlmMessage)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AskQuestion_LLMFailure_ShouldHandleGracefullyAndReturnFallback()
    {
        // Arrange
        var request = new QuestionRequest { Question = "Break Me", UserSessionId = Guid.NewGuid() };
        var messageEntity = new Message(request.UserSessionId, true, "Break Me");
        var finalResponse = new MessageResponse { Id = Guid.NewGuid(), UserSessionId = request.UserSessionId, Content ="The AI service is currently unavailable. Please try again later." , Role = true , Timestamp =  DateTime.Now };

        A.CallTo(() => _fakeMapper.Map<Message>(request)).Returns(messageEntity);

        // Simulating dependency exception throwing
        A.CallTo(() => _fakeLlm.Ask(A<string>._)).Throws(new Exception("API Timeout"));

        A.CallTo(() => _fakeMapper.Map<MessageResponse>(A<Message>.That.Matches(msg => msg.Content.Contains("unavailable"))))
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
        var UserSessionId = Guid.NewGuid();
        var requestDto = new CreateQuizRequest { UserSessionId = UserSessionId, Topic = "Design Patterns" };
        var quizRequestEntity = new QuizRequest { Topic = "Design Patterns" };
        var mockLlmQuizResult = new Quiz {Topic = "Desing Patterns", Items = [new() {QuestionText = "Define Mocking" }
            ]
        };
        var expectedResponse = new QuizRequestResponse { Id = A.Dummy<Guid>(), Topic = "Desing Patterns", QuizItems = new List<QuizItemResponse> { new QuizItemResponse { Id = 0, QuestionText = "Define Mocking" } } };

        A.CallTo(() => _fakeMapper.Map<QuizRequest>(requestDto)).Returns(quizRequestEntity);
        A.CallTo(() => _fakeLlm.CreateQuiz(quizRequestEntity)).Returns(mockLlmQuizResult);
        A.CallTo(() => _fakeMapper.Map<QuizRequestResponse>(quizRequestEntity)).Returns(expectedResponse);

        // Act
        var result = await _chatService.RequestQuiz(requestDto);

        // Assert
        Assert.NotNull(result);
        A.CallTo(() => _fakeSessionRepo.AddQuizRequest(quizRequestEntity)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task EvaluateAnswer_ShouldReturnFeedback()
    {
        // Arrange
        var request = new AnswerEvaluationRequest { QuestionText = "1+1?", UserAnswer = "2" };
        var expectedFeedback = "Correct response!";

        A.CallTo(() => _fakeLlm.Evaluate(request.QuestionText, request.UserAnswer)).Returns(expectedFeedback);

        // Act
        var result = await _chatService.EvaluateAnswer(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCorrect);
        Assert.Equal(expectedFeedback, result.Feedback);
    }
}
