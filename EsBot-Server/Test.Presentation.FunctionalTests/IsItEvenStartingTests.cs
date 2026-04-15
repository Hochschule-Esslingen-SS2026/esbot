using System.Net;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests;

public class IsItEvenStartingTests: IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;

    public IsItEvenStartingTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Swagger_Doc_IsAccessible_SmokeTest()
    {
        var response = await _client.GetAsync("/openapi/v1.json");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, because: $"API failed with: {content}");
    }
}