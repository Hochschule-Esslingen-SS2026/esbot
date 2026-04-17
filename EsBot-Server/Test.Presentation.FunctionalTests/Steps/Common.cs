using System.Net;
using Reqnroll;
using Test.Presentation.FunctionalTests.Context;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests.Steps;

[Binding]
public class Common
{
    private readonly HttpClient _client;
    private readonly TestContext _context;

    public Common(ApiFactory factory, TestContext context)
    {
        _client = factory.CreateClient();
        _context = context;
    }

    [Given(@"the API is running")]
    public async Task GivenTheApiIsRunning()
    {
        var response = await _client.GetAsync("/openapi/v1.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"the response status should be (.*)")]
    public void ThenStatusShouldBe(int statusCode)
    {
        ((int)_context.Response.StatusCode).Should().Be(statusCode);
    }
}