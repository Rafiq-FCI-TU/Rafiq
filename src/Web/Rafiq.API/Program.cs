using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rafiq.API;
using Rafiq.API.Middlewares;
using Rafiq.API.ServicesExtension;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Domain.Settings;
using Rafiq.Infrastructure.Services;
using Rafiq.Persistence.Data;
using Rafiq.Persistence.Identity.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.AddDependencies();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

builder.Services.AddDbContext<RafiqDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();

#endregion

#region Validation Error - Bad Request
// -- Validation Error (Bad Request) 
// --- First: We need to bring options which have InvalidModelState
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // --- then we need all data (actionContext) of action has validation error
    options.InvalidModelStateResponseFactory = (actionContext) =>
    {
        // --- then we bring ModelState: Dictionary key/value pair for each parameter, and value has property Errors Array have all errors
        // --- and we use where to bring dictionary key/value pair which is value has errors 
        var errors = actionContext.ModelState.Where(p => p.Value!.Errors.Count > 0)
            // --- then we use SelectMany to make one array of all error  
            .SelectMany(p => p.Value!.Errors)
            // --- then we use Select to bring from errors just ErrorMessages
            .Select(e => e.ErrorMessage)
            .ToArray();

        // --- then we insert these errors to the class we made
        var validationErrorResponse = new Error(400)
        {
            Message = string.Join("; ", errors)
        };
        // then return it :)
        return new BadRequestObjectResult(validationErrorResponse);
    };
});
#endregion

var app = builder.Build();

#region Update Database With Using Way And Seeding Data

// We Said To Update Database You Should Do Two Things (1. Create Instance From DbContext 2. Migrate It)

// To Ask Clr To Create Instance Explicitly From Any Class
//    1 ->  Create Scope (Life Time Per Request)
using var scope = app.Services.CreateScope();
//    2 ->  Bring Service Provider Of This Scope
var services = scope.ServiceProvider;

// --> Bring Object Of StoreContext For Update His Migration
var rafiqContext = services.GetRequiredService<RafiqDbContext>();
// --> Bring Object Of ILoggerFactory For Good Show Error In Console
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    // Migrate StoreContext
    await rafiqContext.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error has been occured during apply the migration!");
}

#endregion

#region Configure the Kestrel pipeline

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndOpenApiMiddleware();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.MapControllers();

#endregion

app.Run();
