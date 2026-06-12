using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Test.Presentation.FunctionalTests.Context;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests;


public class ExtraTests : IClassFixture<ApiFactory>
{

    private readonly HttpClient _client;
    private readonly TestContext _context;

    public ExtraTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<TestContext>();
    }

    [Fact]
    public async Task HappyPath()
    {
        var response = await _client.GetAsync("api/v1/health");
        response.EnsureSuccessStatusCode();

        var payload = new CreateSessionRequest("UserName");
        response = await _client.PostAsJsonAsync("api/v1/sessions", payload);
        response.EnsureSuccessStatusCode();

        var responseMessage = Deserialize<SessionResponse>(response.Content.ReadAsStringAsync().Result);
        Guid sessionId = responseMessage.SessionId;

        var questionRequest = new QuestionRequest
        {
            UserSessionId = sessionId,
            Question = "QuestionAutokamtedTest"
        };
        response = await _client.PostAsJsonAsync($"api/v1/sessions/{sessionId}/messages", questionRequest);
        response.EnsureSuccessStatusCode();

        response = await _client.GetAsync($"api/v1/sessions/messages/{sessionId}");
        response.EnsureSuccessStatusCode();

        var messages = Deserialize<IEnumerable<MessageResponse>>(response.Content.ReadAsStringAsync().Result);
        messages.Should().HaveCount(2);
        messages.First().UserSessionId.Should().Be(sessionId);

    }

    [Fact]
    public async Task WrongApiPath()
    {
        var response = await _client.GetAsync("api/v1/healthaa");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WrongPayload()
    {
        var payload = new QuestionRequest
        {
            UserSessionId = Guid.NewGuid(),
            Question = "QuestionAutokamtedTest"
        };
        var response = await _client.PostAsJsonAsync("api/v1/sessions", payload);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetMessagesWithEmptyList()
    {
        var payload = new CreateSessionRequest("UserName");
        var response = await _client.PostAsJsonAsync("api/v1/sessions", payload);
        response.EnsureSuccessStatusCode();

        var responseMessage = Deserialize<SessionResponse>(response.Content.ReadAsStringAsync().Result);
        Guid sessionId = responseMessage.SessionId;

        var messagesResponse = await _client.GetAsync($"api/v1/sessions/messages/{sessionId}");
        messagesResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        messagesResponse.ReasonPhrase.Should().Be("Not Found");
    }

    [Fact]
    public async Task TryAskingForANSFWQuiz()
    {
        var payload = new CreateSessionRequest("UserName");
        var response = await _client.PostAsJsonAsync("api/v1/sessions", payload);
        response.EnsureSuccessStatusCode();

        var quizRequest = new QuizRequest
        {
            UserSessionId = Guid.NewGuid(),
            Topic = "NSFW"
        };

        var quizResponse = await _client.PostAsJsonAsync("api/v1/quiz", quizRequest);
        quizResponse.StatusCode.Should().Be(HttpStatusCode.PaymentRequired);
    }

    private T Deserialize<T>(string content) =>
        JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

}
