using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Shared.Models
{
    [ExcludeFromCodeCoverage]
    public class CacheOptions
    {
        public int AbsoluteExpiration { get; set; }
        public int SlidingExpiration { get; set; }
        public CacheItemPriority Priority { get; set; }
    }
}
