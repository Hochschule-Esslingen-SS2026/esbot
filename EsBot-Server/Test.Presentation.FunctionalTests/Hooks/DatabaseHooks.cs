using Test.Presentation.FunctionalTests.Helper;

namespace Test.Presentation.FunctionalTests.Hooks;

public class DatabaseHooks
{
    private readonly ApiFactory _factory;

    public DatabaseHooks(ApiFactory factory)
    {
        _factory = factory;
    }

}