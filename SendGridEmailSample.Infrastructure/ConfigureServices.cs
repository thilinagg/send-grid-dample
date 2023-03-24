using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Infrastructure.Configs;
using SendGridEmailSample.Infrastructure.Persistence;
using SendGridEmailSample.Infrastructure.Persistence.Interceptors;
using SendGridEmailSample.Infrastructure.Services;

namespace SendGridEmailSample.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<SendGridConfigs>().Configure<IConfiguration>((SendGridConfigs, config) =>
        {
            config.GetSection(nameof(SendGridConfigs)).Bind(SendGridConfigs);
        });

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IEmailSenderService, EmailSenderService>();
        return services;
    }
}
