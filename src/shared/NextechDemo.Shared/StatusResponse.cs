using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Shared
{
    [ExcludeFromCodeCoverage]
    public class StatusResponse
    {
        public string ExceptionType { get; set; }
        public string ErrorMessage { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
