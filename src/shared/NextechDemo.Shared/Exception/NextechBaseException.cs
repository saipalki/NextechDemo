using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace NextechDemo.Shared.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class NextechBaseException : ApplicationException
    {
        public NextechBaseException() { }

        /// <summary>
        /// custom exception handling on server side and to show the error message over UI
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        public NextechBaseException(string message, ExceptionType exceptionDispositionType, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = (int)statusCode;
            ExceptionDispositionType = exceptionDispositionType;
        }
        /// <summary>
        /// custom exception handling on server side and to show the error message over UI
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="statusCode"></param>
        public NextechBaseException(string message, ExceptionType exceptionDispositionType, Exception ex, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, ex)
        {
            StatusCode = (int)statusCode;
            ExceptionDispositionType = exceptionDispositionType;
        }
        /// <summary>
        ///     To show validation errors
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="statusCode"></param>
        public NextechBaseException(ExceptionType exceptionDispositionType = ExceptionType.ValidationError, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            StatusCode = (int)statusCode;
            ExceptionDispositionType = exceptionDispositionType;
            
        }
        public int StatusCode { get; }
        public ExceptionType ExceptionDispositionType { get; }
        
    }
}
