using System.Net.Http.Json;
using Core.Data.DTOs.Requests;
using Reqnroll;
using Test.Presentation.FunctionalTests.Context;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests.Steps;

[Binding]
public class AskQuestion
{
    private readonly HttpClient _client;
    private readonly TestContext _context;

    public AskQuestion(ApiFactory factory, TestContext context)
    {
        _client = factory.CreateClient();
        _context = context;
    }

    [When("the Student sends a question")]
    public async Task WhenTheStudentSendsAQuestion()
    {
        var userSessionId = Guid.NewGuid();
        var payload = new QuestionRequest
        {
            Question = "What is polymorphism?",
            UserSessionId = userSessionId
        };

        var response = await _client.PostAsJsonAsync("/api/v1/question", payload);

        _context.Response = response;
        _context.ResponseContent = await response.Content.ReadAsStringAsync();
        _context.SessionId = userSessionId;
    }
}