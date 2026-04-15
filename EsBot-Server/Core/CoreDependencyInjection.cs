using Core.Interfaces.Services;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageManagementService, MessageManagementService>();
        
        services.AddAutoMapper(cfg => {},typeof(IApplicationAssemblyMarker).Assembly);
        
        return services;
    }
}