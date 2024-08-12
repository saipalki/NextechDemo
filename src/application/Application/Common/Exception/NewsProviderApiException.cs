namespace NextechDemo.Application.Common.Exception
{
    /// <summary>
    ///     Custom exception
    /// </summary>
    public class NewsProviderApiException : System.Exception
    {
        public string Message { get; set; }
        public int Status { get; set; }
        
        public NewsProviderApiException()
        {
        }
        public NewsProviderApiException(string message, int statusCode)
        {
            Message = message;
            Status = statusCode;
        }
    }
}
