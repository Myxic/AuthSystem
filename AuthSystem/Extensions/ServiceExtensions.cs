
using System.Text.Json.Serialization;
using AuthSystem.core.Interfaces.Auth;
using AuthSystem.core.Interfaces.Email;
using AuthSystem.Core.Infrastructure.DataContext;
using AuthSystem.Infrastructure.Authorization;
using AuthSystem.Services.Impentation.Auth;
using AuthSystem.Services.Impentation.Email;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLoggerService(this IServiceCollection services)
    {
     
        
        



        // configure DI for application services
        services.AddScoped<IJwtUtils, JwtUtils>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmailService, EmailService>();

    }

    public static void AddDBConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("WebApiDatabase")
     ));

        services.AddLogging(builder =>
        {
            builder.AddConsole(); // Add console logger
        });


    }





}
