using NextechDemo.Api.Containers;
using NextechDemo.Api.Middleware;
using NextechDemo.Shared.Models;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            // Add services to the container.
            var configuration = builder.Configuration;

            builder.Services.AddServicesInAssembly(configuration);
            //services cors
            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddControllers();
            builder.Services.AddScoped(sp => builder.Configuration.GetSection("CacheOptions").Get<CacheOptions>());
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            //Enable Swagger and SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nextech Demo v1"); });

            app.UseAuthorization();
            app.UseAppException();

            app.MapControllers();
            app.UseCors("corsapp");

            app.Run();
        }
    }
}