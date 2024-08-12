using NextechDemo.Application.Config;
using NextechDemo.Application.Services.CacheService;
using NextechDemo.Application.Services.NewsProvider;
using SampleAssignment.Api.Containers;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection;

namespace NextechDemo.Api.Containers
{
    /// <summary>
    ///     Application service registration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegisterApplicationServices : IServiceRegistration
    {
        /// <summary>
        ///     Register application to service collection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Register a typed httpclient for shipping provider service
            services.AddHttpClient<INewsProviderService, NewsProviderService>(client =>
            {
                client.BaseAddress = new Uri(configuration[$"ServiceUrls:NewsProvidersApi"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddScoped<ICacheService, CacheService>();
            services.AddMemoryCache();
        }
}
}
