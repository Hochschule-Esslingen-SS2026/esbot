using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Presentation.FunctionalTests.Context;

namespace Test.Presentation.FunctionalTests.Helper;


public class ApiFactory : WebApplicationFactory<Program>
{
    public string DbName { get; } = Guid.NewGuid().ToString();
    
    public Action<IServiceCollection>? ConfigureTestServicesAction { get; set; }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(DbName);
            });
            ConfigureTestServicesAction?.Invoke(services);
            services.AddScoped<TestContext>();
        });

        builder.UseEnvironment("Testing");
    }
}