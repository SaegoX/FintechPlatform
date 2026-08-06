using LoanService.API.Infrastructure;
using LoanService.Application.Common;
using LoanService.Application.UseCases.LoanApplications;
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


            builder.Services.AddSingleton<ILoanRepository, InMemoryLoanRepository>();
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
