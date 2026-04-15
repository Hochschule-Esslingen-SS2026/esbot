using Core.Data.Entities;

namespace Test.Application.UnitTests.Data;

public class QuizRequestTests
{

    [Fact]
    public void QuizRequest_ValidTopic_Passes()
    {
        var request = new QuizRequest { Topic = "Software Testing", UserSessionId = Guid.NewGuid() };
        var errors = Data.ValidationHelper.ValidateModel(request);
        errors.Should().BeEmpty();
    }

    [Fact]
    public void QuizRequest_TopicTooLong_Fails()
    {
        var request = new QuizRequest { Topic = new string('x', 201) };
        var errors = Data.ValidationHelper.ValidateModel(request);
        errors.Should().Contain(e => e.MemberNames.Contains("Topic"));
    }
}