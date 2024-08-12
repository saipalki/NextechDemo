using Microsoft.OpenApi.Models;
using SampleAssignment.Api.Containers;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Api.Containers
{
    /// <summary>
    ///     Register swagger
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegisterSwagger : IServiceRegistration
    {
        /// <summary>
        ///     Swagger registration to service collection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            //Register Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Nextech Demo", Version = "v1" });
                options.CustomSchemaIds(type => type.ToString());

            });
        }
    }
}
