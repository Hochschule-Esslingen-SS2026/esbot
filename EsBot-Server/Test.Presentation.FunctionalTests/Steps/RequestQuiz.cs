using System.Net.Http.Json;
using System.Text.Json;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Reqnroll;
using Test.Presentation.FunctionalTests.Context;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests.Steps;

[Binding]
public class RequestQuiz
{
    private readonly HttpClient _client;
    private readonly TestContext _context;

    public RequestQuiz(ApiFactory factory, TestContext context)
    {
        _client = factory.CreateClient();
        _context = context;
    }

    [When(@"the Student requests a quiz on ""(.*)""")]
    public async Task WhenTheStudentRequestsAQuizOn()
    {
        var userSessionId = Guid.NewGuid();
        var payload = new CreateQuizRequest
        {
            Topic = "Java",
            UserSessionId = userSessionId
        };

        var response = await _client.PostAsJsonAsync("/v1/quiz", payload);

        _context.Response = response;
        _context.ResponseContent = await response.Content.ReadAsStringAsync();
        _context.SessionId = userSessionId;

    }

    [Then(@"the System generates a list of questions")]
    public void ThenSystemGeneratesAListOfQuestions()
    {
        var questions = JsonSerializer.Deserialize<QuizRequestResponse>(
            _context.ResponseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        questions.Should().NotBeNull();
        questions!.QuizItems.Should().NotBeEmpty();
    }

    [Then(@"the questions are send to the student")]
    public void ThenQuestionsAreSentToTheStudent()
    {
        _context.ResponseContent.Should().NotBeNullOrEmpty();
    }
}