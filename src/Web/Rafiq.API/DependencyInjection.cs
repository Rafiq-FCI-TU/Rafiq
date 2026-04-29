using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Rafiq.API.ServicesExtension;
using Rafiq.Application.Hubs;
using Rafiq.Application.Interfaces;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Services;
using Rafiq.Domain.Entities;
using Rafiq.Infrastructure.Services;
using Rafiq.Infrastructure.Services.Auth;
using Rafiq.Infrastructure.Services.Patients;
using Rafiq.Infrastructure.Services.Sessiones;
using Rafiq.Infrastructure.Services.Specialists;
using Rafiq.Infrastructure.Settings;
using Rafiq.Persistence;
using Rafiq.Persistence.Data;

namespace Rafiq.API;
public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFamilyRegistrationService, FamilyRegistrationService>();
        services.AddScoped<ISpecialistRegistrationService, SpecialistRegistrationService>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IAssessmentService, AssessmentService>();
        services.AddScoped<IGamingService, GamingService>();
        services.AddScoped<ICommunityService, CommunityService>();
        services.AddScoped<IPatientSessionAttemptService, PatientSessionAttemptService>();
        services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddSignalR();

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<RafiqDbContext>());
        services.AddScoped<IRealtimeSessionService, RealtimeSessionService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ICommunityService, CommunityService>();
        builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

        builder.Services.AddScoped<ISpecialistRepository, SpecialistRepository>();
        builder.Services.AddScoped<ISpecialistService, SpecialistService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IRealtimeSessionService, RealtimeSessionService>();

        builder.Services.AddSingleton<BlobHelperService>();



        builder.Services.Configure<AzureBlobSettings>(
    builder.Configuration.GetSection("AzureBlobStorage"));

        builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<AzureBlobSettings>>().Value
        );

        services.AddControllers();


        services.AddSwaggerServices();
    }
}
