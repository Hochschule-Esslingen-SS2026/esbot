using Core.Data.Entities;

namespace Test.Application.UnitTests.Data;

public class MessageTests
{

    [Fact]
    public void CreateMessage_WithValidData_SetsProperties()
    {
        var message = new Message(Guid.NewGuid(),true,"Explain AI");

        message.Content.Should().Be("Explain AI");
        message.Role.Should().Be(true);
        message.CreatedAt.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
    }

    [Fact]
    public void Message_ContentExceedsReasonableLimits_CheckIfRequired()
    {
        var message = new Message(Guid.NewGuid(),true, null!);
        var errors = UnitTests.ValidationHelper.ValidateModel(message);
        errors.Should().Contain(e => e.MemberNames.Contains("Content"));
    }
}
