using BillingService.Application.Common;
using BillingService.Application.Consumers;
using BillingService.Infrastructure;
using BillingService.Infrastructure.Messaging;
using BillingService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Globalization;



CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("BillingPostgresConnection");
builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IWalletRepository, PostgresWalletRepository>();
builder.Services.AddScoped<ProcessLoanApprovedHandler>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LoanApprovedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("fintech_rabbit");
            h.Password("12356789");
        });

        cfg.ReceiveEndpoint("billing-loan-approved-queue", e =>
        {
            e.ConfigureConsumer<LoanApprovedConsumer>(context);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<BillingDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка наката миграций биллинга: {ex.Message}");
    }
}

app.UseAuthorization();
app.MapControllers();

app.Run();
