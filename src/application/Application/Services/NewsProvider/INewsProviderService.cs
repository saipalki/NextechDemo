using NextechDemo.Shared.Models;
using NextechDemo.Shared;
using NextechDemo.Shared.Models.News;

namespace NextechDemo.Application.Services.NewsProvider
{
    public interface INewsProviderService
    {
        /// <summary>
        ///     List of all top news  
        /// </summary>
        /// <returns></returns>
        Task<Response<List<NewsResponseModel>>> GetNews(PageParams pageParams);
    }
}
