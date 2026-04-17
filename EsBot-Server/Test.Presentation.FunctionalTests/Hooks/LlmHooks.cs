using Core.Interfaces.Services;
using Test.Presentation.FunctionalTests.Helper;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace Test.Presentation.FunctionalTests.Hooks;

[Binding]
public class LlmHooks
{
    private readonly ApiFactory _factory;

    public LlmHooks(ApiFactory factory)
    {
        _factory = factory;
    }

    [BeforeScenario("@mock-llm")]
    public void MockLlm()
    {
        _factory.ConfigureTestServicesAction = services =>
        {
            var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ILlmInterface));
            if (descriptor != null)
                services.Remove(descriptor);

            var fakeLlm = A.Fake<ILlmInterface>();

            A.CallTo(() => fakeLlm.Ask(A<string>._))
                .Returns(Task.FromResult("mockedanswer"));

            services.AddSingleton(fakeLlm);
        };
    }
    
    [BeforeScenario("@llm-timeout")]
    public void MockTimeout()
    {
        _factory.ConfigureTestServicesAction = services =>
        {
            var fake = A.Fake<ILlmInterface>();

            A.CallTo(() => fake.Ask(A<string>._))
                .ThrowsAsync(new TimeoutException());

            services.AddSingleton(fake);
        };
    }
}