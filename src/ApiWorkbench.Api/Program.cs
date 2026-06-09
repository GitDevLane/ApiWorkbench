using System.Text.Json.Serialization;
using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Data.Repositories;
using ApiWorkbench.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

builder.Services.AddScoped<IConnectionTestService, MockConnectionTestService>();
builder.Services.AddScoped<IConnectionProfileValidator, ConnectionProfileValidator>();

builder.Services.AddScoped<IConnectionProfileRepository>(_ =>
{
    var configuredPath = builder.Configuration["ProfileStorage:FilePath"]
        ?? "App_Data/profiles.json";

    var filePath = Path.IsPathRooted(configuredPath)
        ? configuredPath
        : Path.Combine(AppContext.BaseDirectory, configuredPath);

    return new JsonConnectionProfileRepository(filePath);
});

builder.Services.AddScoped<IConnectionTestHistoryRepository>(_ =>
{
    var configuredPath = builder.Configuration["HistoryStorage:FilePath"]
        ?? "App_Data/history.json";

    var filePath = Path.IsPathRooted(configuredPath)
        ? configuredPath
        : Path.Combine(AppContext.BaseDirectory, configuredPath);

    return new JsonConnectionTestHistoryRepository(filePath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
