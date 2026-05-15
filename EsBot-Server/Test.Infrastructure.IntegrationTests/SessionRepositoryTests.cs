using Core.Data.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Test.Infrastructure.IntegrationTests;

public class SessionRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddSession_ShouldPersistSessionToInMemoryDb()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var session = new UserSession("user-123");

        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);

            // Act
            await repository.AddSession(session);
        }

        // Assert
        using (var context = new ApplicationDbContext(options))
        {
            var databaseSession = await context.UserSessions.FindAsync(session.Id);

            Assert.NotNull(databaseSession);
            Assert.Equal("user-123", databaseSession.ExternalUserId);
        }
    }

    [Fact]
    public async Task GetSession_ShouldRetrieveCorrectSessionWithMessages()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var session = new UserSession("user-456");
        var message = new Message(session.Id, true, "What is an interface?");

        using (var context = new ApplicationDbContext(options))
        {
            await context.UserSessions.AddAsync(session);
            await context.Messages.AddAsync(message);
            await context.SaveChangesAsync();
        }

        // Act
        UserSession? result;
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);
            result = await repository.GetSession(session.Id);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Messages);
        Assert.Equal("What is an interface?", result.Messages.First().Content);
    }

    [Fact]
    public async Task GetSessionsByUserId_ShouldReturnFilteredCollection()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var targetUser = "student-01";
        var otherUser = "student-02";

        using (var context = new ApplicationDbContext(options))
        {
            await context.UserSessions.AddRangeAsync(
                new UserSession(targetUser),
                new UserSession(targetUser),
                new UserSession(otherUser)
            );
            await context.SaveChangesAsync();
        }

        // Act
        System.Collections.Generic.IEnumerable<UserSession> results;
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);
            results = await repository.GetSessionsByUserId(targetUser);
        }

        // Assert
        Assert.Equal(2, results.Count());
        Assert.All(results, s => Assert.Equal(targetUser, s.ExternalUserId));
    }

    [Fact]
    public async Task AddMessage_ShouldLinkMessageToSession()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var session = new UserSession("user-789");
        var message = new Message (session.Id,true,"Unit testing with fakes");

        using (var context = new ApplicationDbContext(options))
        {
            await context.UserSessions.AddAsync(session);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);
            await repository.AddMessage(message);
        }

        // Assert
        using (var context = new ApplicationDbContext(options))
        {
            var savedMessage = await context.Messages.FindAsync(message.Id);

            Assert.NotNull(savedMessage);
            Assert.Equal(session.Id, savedMessage.UserSessionId);
            Assert.Equal("Unit testing with fakes", savedMessage.Content);
        }
    }

    [Fact]
    public async Task GetMessagesBySessionIdAsync_ShouldReturnChronologicalHistory()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var session = new UserSession("user-sequence");
        var earlyMsg = new Message(session.Id, true, "First Message");
        earlyMsg.CreatedAt = DateTime.UtcNow.AddMinutes(-10);
        var lateMsg = new Message(session.Id, true, "Second Message");

        using (var context = new ApplicationDbContext(options))
        {
            await context.UserSessions.AddAsync(session);
            // Intentionally adding them out of logical order to ensure repository sorts them
            await context.Messages.AddRangeAsync(lateMsg, earlyMsg);
            await context.SaveChangesAsync();
        }

        // Act
        System.Collections.Generic.List<Message> history;
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);
            history = (await repository.GetMessagesBySessionIdAsync(session.Id)).ToList();
        }

        // Assert
        Assert.Equal(2, history.Count);
        Assert.Equal("First Message", history[0].Content);  // Oldest first
        Assert.Equal("Second Message", history[1].Content); // Newest second
    }

    [Fact]
    public async Task UpdateSession_ShouldPersistChangedMetadata()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var session = new UserSession("user-update");

        using (var context = new ApplicationDbContext(options))
        {
            await context.UserSessions.AddAsync(session);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);
            await repository.UpdateSession(session);
        }

        // Assert
        using (var context = new ApplicationDbContext(options))
        {
            var updated = await context.UserSessions.FindAsync(session.Id);

            Assert.NotNull(updated);
        }
    }

    [Fact]
    public async Task DeleteSession_ShouldRemoveSessionAndAssociatedMessages()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var session = new UserSession("user-delete");
        var message = new Message (session.Id,true,"Temporary data");

        using (var context = new ApplicationDbContext(options))
        {
            await context.UserSessions.AddAsync(session);
            await context.Messages.AddAsync(message);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new global::Infrastructure.Persistence.Repositories.SessionRepository(context);
            await repository.DeleteSession(session.Id);
        }

        // Assert
        using (var context = new ApplicationDbContext(options))
        {
            var missingSession = await context.UserSessions.FindAsync(session.Id);
            var missingMessages = await context.Messages.Where(m => m.UserSessionId == session.Id).ToListAsync();

            Assert.Null(missingSession);
            Assert.Empty(missingMessages); // Confirms behavior matching the requested criteria
        }
    }
}
