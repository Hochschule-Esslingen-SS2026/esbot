namespace Test.Presentation.FunctionalTests.Context;

public class TestContext
{
    public HttpResponseMessage Response { get; set; }
    public string ResponseContent { get; set; }
    public Guid SessionId { get; set; }
}