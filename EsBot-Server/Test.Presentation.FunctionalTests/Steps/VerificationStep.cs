using Infrastructure.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Test.Presentation.FunctionalTests.Context;
using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests.Steps;

[Binding]
public class VerificationSteps
{
    private readonly ApiFactory _factory;
    private readonly TestContext _context;

    public VerificationSteps(ApiFactory factory, TestContext context)
    {
        _factory = factory;
        _context = context;
    }

    [Then(@"the question should be saved with the session-id in the DataBase")]
    public void ThenQuestionShouldBeSaved()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var exists = db.Messages.Any(m => m.UserSessionId == _context.SessionId);

        exists.Should().BeTrue();
    }

    [Then(@"the respone should contain ""(.*)""")]
    public void ThenResponseShouldContain(string expected)
    {
        _context.ResponseContent.Should().Contain(expected);
    }
}