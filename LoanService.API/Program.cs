using LoanService.Application.Common;
using LoanService.Application.UseCases.LoanApplications;
using LoanService.Infrastructure;
using LoanService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Globalization;

namespace LoanService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

            builder.Services.AddScoped<ILoanRepository, PostgresLoanRepository>();

            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateLoanApplicationCommand).Assembly));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.MapScalarApiReference();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
