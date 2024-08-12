using NextechDemo.Application.Common.Exception;
using NextechDemo.Shared;
using NextechDemo.Shared.Exceptions;
using NextechDemo.Shared.Helper;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NextechDemo.Api.Middleware
{
    /// <summary>
    /// Exception middleware to handle exception globally
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext )
        {
            int? statusCode = null;
            ExceptionType exceptionType = ExceptionType.FatalError;
            string? methodName = null;

            try
            {
                await _next(httpContext);
            }
            catch (NextechBaseException nbe)
            {
                _logger.LogError(nbe, nbe.Message);
                statusCode = nbe.StatusCode;
                exceptionType = nbe.ExceptionDispositionType;
                methodName = httpContext.Request.Path;
                // Handle exception and send to client
                await HandleExceptionAsync(httpContext, nbe, methodName, exceptionType, statusCode);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                statusCode = default;
                methodName = httpContext.Request.Path;
                // Handle exception and send to client
                await HandleExceptionAsync(httpContext, ex, methodName, exceptionType, statusCode);
            }
        }

        /// <summary>
        /// Exception details to be sent to client
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="methodName"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string? methodName, ExceptionType exceptionType, int? statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode == null ? GetStatusCode(exception) : statusCode.Value;
            var message = exception switch
            {
                NextechBaseException => HandleNextechBaseException(exception),
                NewsProviderApiException => HandleNewsProviderApiException(exception),
                _ => AppConstants.GenericMessageTemplate
            };

            exceptionType = GetExceptionType(exception, exceptionType);
            await context.Response.WriteAsync(new StatusResponse
            {
                ErrorMessage = message,
                ExceptionType = Enum.GetName(exceptionType)!
            }.ToString());
        }
        /// <summary>
        ///     Handle back office base exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static string HandleNextechBaseException(Exception exception)
        {
            var message = new StringBuilder();
            var nextechBaseException = exception as NextechBaseException;
                message.Append(nextechBaseException.Message);
             
            return message.ToString();
        }
        /// <summary>
        ///     Handle shipping provider exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static string HandleNewsProviderApiException(Exception exception)
        {
            var message = new StringBuilder();
            if (exception is NewsProviderApiException apiException)
            {
                    message.Append(apiException.Message);
            }
            return message.ToString();
        }
        /// <summary>
        /// Get status code
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static int GetStatusCode(Exception exception) =>
        exception switch
        {
          
            NewsProviderApiException => (exception as NewsProviderApiException)!.Status,
            _ => StatusCodes.Status500InternalServerError
        };
        /// <summary>
        /// Get exception type from type of exception
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        private static ExceptionType GetExceptionType(Exception exception, ExceptionType exceptionType) =>
        exception switch
        {
              NewsProviderApiException => ExceptionType.ExternalApiError,
            _ => exceptionType
        };
    }
}
