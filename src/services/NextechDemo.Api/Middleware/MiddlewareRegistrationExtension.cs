using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Api.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class MiddlewareRegistrationExtension
    {
        /// <summary>
        /// Register exception middleware
        /// </summary>
        /// <param name="builder"></param>
        public static void UseAppException(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
