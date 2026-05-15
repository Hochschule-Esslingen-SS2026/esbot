using System.Text.Json;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Infrastructure.Persistence.Context;
using Reqnroll;
using Test.Presentation.FunctionalTests.Context;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests.Steps;

[Binding]
public class ResumeSession
{
    private readonly HttpClient _client;
    private readonly TestContext _context;
    private readonly ApiFactory _factory;

    public ResumeSession(TestContext context, ApiFactory factory)
    {
        _client = factory.CreateClient();
        _context = context;
        _factory = factory;
    }

    [Given(@"the database is seeded with messages")]
    public async Task SeedJavaQuestions()
    {
        var sessionId = Guid.NewGuid();
        _context.SessionId = sessionId;
        await _factory.SeedDataAsync<ApplicationDbContext>(db =>
        {
            db.Messages.AddRange(new List<Message>
            {
                new( sessionId,false,"Bllaaa"),
                new( sessionId,false,"Bllaaa1"),
                new( sessionId,false,"Bllaaa2"),
                new( sessionId,false,"Bllaaa3"),
            });
        });
    }

    [When(@"the student requests an old session with session-id")]
    public async Task WhenTheStudentRequestsAnOldSessionWithSessionId()
    {
        var response = await _client.GetAsync($"API/v1/Session?sessionId={_context.SessionId}");

        _context.Response = response;
        _context.ResponseContent = await response.Content.ReadAsStringAsync();
    }

    [Then(@"the response should contain all messages with that session-id")]
    public void ThenTheResponseShouldContainAllMessagesWithThatSessionId()
    {
        var messages = Deserialize<IEnumerable<MessageResponse>>(_context.ResponseContent);

        messages.Should().NotBeNull();
        messages.Should().HaveCount(3);
        foreach (var message in messages)
        {
            message.UserSessionId.Should().Be(_context.SessionId);
        }
    }

    [When(@"the student request an session with a unknown session-id")]
    public async Task WhenTheStudentRequestAnSessionWithAUnknownSessionId()
    {
        var userSessionId = Guid.NewGuid();
        var response = await _client.GetAsync($"API/v1/Session?id={userSessionId}");

        _context.Response = response;
        _context.ResponseContent = await response.Content.ReadAsStringAsync();
    }

    [Then(@"the response should contain ""(.*)""")]
    public void ThenTheResponseShouldContainDidNotFindYourSession(string expectedMessage)
    {
        _context.ResponseContent.Should().Contain(expectedMessage);
    }

    private T Deserialize<T>(string content) =>
        JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

}
