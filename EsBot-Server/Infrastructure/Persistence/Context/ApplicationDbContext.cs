using Core.Data.DTOs.Requests;
using Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<QuizRequest> QuizRequests { get; set; }
    public DbSet<QuizItem> QuizItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Session to Messages (One-to-Many)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.UserSession)
            .WithMany(s => s.Messages)
            .HasForeignKey(m => m.UserSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // 2. Session to QuizRequests (One-to-Many)
        modelBuilder.Entity<QuizRequest>()
            .HasOne(q => q.UserSession)
            .WithMany(s => s.QuizRequests)
            .HasForeignKey(q => q.UserSessionId);

        // 3. QuizRequest to QuizItems (One-to-Many)
        modelBuilder.Entity<QuizItem>()
            .HasOne(i => i.QuizRequest)
            .WithMany(r => r.QuizItems)
            .HasForeignKey(i => i.QuizRequestId);

        // Indexing for Performance (NFR-2)
        modelBuilder.Entity<UserSession>()
            .HasIndex(s => s.ExternalUserId);

        // Configure UserSession -> Messages cascade
        modelBuilder.Entity<UserSession>()
            .HasMany(s => s.Messages)
            .WithOne(m => m.UserSession) // Assumes Message has a 'UserSession' or 'UserSessionId' property
            .HasForeignKey(m => m.UserSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure UserSession -> QuizRequests cascade
        modelBuilder.Entity<UserSession>()
            .HasMany(s => s.QuizRequests)
            .WithOne(q => q.UserSession) // Assumes QuizRequest has a 'UserSession' or 'UserSessionId' property
            .HasForeignKey(q => q.UserSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
