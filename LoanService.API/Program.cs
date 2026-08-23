using LoanService.Application.Common;
using LoanService.Application.UseCases.LoanApplications;
using LoanService.Infrastructure;
using LoanService.Infrastructure.Caching;
using LoanService.Infrastructure.Messaging;
using LoanService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Globalization;


CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
     });

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<LoanDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
builder.Services.AddScoped<ILoanRepository, PostgresLoanRepository>();

builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssembly(typeof(CreateLoanApplicationCommand).Assembly));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("fintech_rabbit");
            h.Password("12356789");
        });
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["REDIS_CONNECTION"] ?? "redis:6379";
    options.InstanceName = "LoanService_";
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LoanDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при автоматическом накате миграций на Postgres.");
    }

    app.Run();
}