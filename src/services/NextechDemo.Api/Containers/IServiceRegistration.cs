using System.Diagnostics.CodeAnalysis;

namespace SampleAssignment.Api.Containers
{

    public interface IServiceRegistration
    {
        void RegisterAppServices(IServiceCollection services, IConfiguration configuration);
    }
}
