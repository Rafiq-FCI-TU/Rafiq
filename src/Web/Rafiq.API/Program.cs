using Rafiq.API;
using Rafiq.API.ServicesExtension;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.AddDependencies();

#endregion

var app = builder.Build();

#region Configure the Kestrel pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndOpenApiMiddleware();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
