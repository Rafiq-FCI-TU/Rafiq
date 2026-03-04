using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rafiq.API;
using Rafiq.API.Middlewares;
using Rafiq.API.ServicesExtension;
using Rafiq.Application.MappingProfıles;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Domain.Settings;
using Rafiq.Persistence.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.AddDependencies();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

#endregion

#region Services Registration

//builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(Rafiq.Application.AssemblyReference).Assembly));



#endregion

#region Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequireNonAlphanumeric = true;
    option.Password.RequiredUniqueChars = 3;
    option.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<RafiqDbContext>()
.AddDefaultTokenProviders();

#endregion

//#region Swagger Configuration 
//builder.Services.AddSwaggerGen(c =>
//{
//    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
//    foreach (var xmlFile in xmlFiles)
//    {
//        c.IncludeXmlComments(xmlFile);
//    }

//    c.EnableAnnotations();

//    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "Rafiq API",
//        Version = "v1",
//        Description = @"
//Rafiq API is a comprehensive platform for managing care services for children with Down syndrome. 
//It provides a full set of endpoints to handle all aspects of patient care, family management, appointments, and clinical operations.

//This API is developed as a graduation project aimed at improving care for children with special needs.",
//        Contact = new Microsoft.OpenApi.Models.OpenApiContact
//        {
//            Name = "Support",
//            Email = "abdulrahmanfikry1@gmail.com",
//            Url = new Uri("https://yourwebsite.com")
//        },
//        License = new Microsoft.OpenApi.Models.OpenApiLicense
//        {
//            Name = "MIT License",
//            Url = new Uri("https://opensource.org/licenses/MIT")
//        }
//    });

//});



//#endregion

#region Jwt Configuration

// Configure JWT settings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

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
var databaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// register dbcontext
builder.Services.AddDbContext<RafiqDbContext>(options =>
    options.UseSqlServer(databaseConnectionString));
#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

#endregion

#region Redis Configuration

var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost";

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));

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

    // Seed Data To StoreContext
    await RafiqDbContextSeed.SeedAsync(rafiqContext);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error has been occured during apply the migration!");
}

#endregion

#region Configure the Kestrel pipeline

app.UseMiddleware<ExceptionMiddleware>();


app.UseSwaggerAndOpenApiMiddleware();

app.UseStaticFiles();
    
app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.MapControllers();

#endregion

app.Run();

