using SampleAssignment.Api.Containers;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Api.Containers
{
    [ExcludeFromCodeCoverage]
    public static class ServiceRegistrationExtension
    {
        /// <summary>
        ///     Add all services available in service container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var appServices = typeof(Program).Assembly.DefinedTypes
                 .Where(x => typeof(IServiceRegistration)
                                 .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(Activator.CreateInstance)
                 .Cast<IServiceRegistration>().ToList();

            appServices.ForEach(svc => svc.RegisterAppServices(services, configuration));
        }
    }
}
