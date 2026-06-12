using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Provide your local development connection string here
        optionsBuilder.UseNpgsql("Host=local4.braincrush.org;Database=esbot;Username=esbot;Password=esbotpassword");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
