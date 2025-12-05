using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rafiq.API;
using Rafiq.API.Middlewares;
using Rafiq.API.ServicesExtension;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Persistence.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.Services.AddAutoMapper(typeof(Program));

builder.AddDependencies();

#endregion

#region Services Registration

//builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // v1, v2
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(Rafiq.Application.AssemblyReference).Assembly));



#endregion

#region Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<RafiqDbContext>()
.AddDefaultTokenProviders();
#endregion

#region Jwt Configuration
builder.Services.AddJwtSettings(builder.Configuration);

#endregion

#region Controllers & API Configuration

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

#region Database Configuration
// read connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// register dbcontext
builder.Services.AddDbContext<RafiqDbContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

#region Redis Configuration

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect("localhost"));

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
