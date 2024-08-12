using Microsoft.Extensions.Logging;
using NextechDemo.Application.Common.Exception;
using NextechDemo.Application.Config;
using NextechDemo.Application.Helper;
using NextechDemo.Application.Services.CacheService;
using NextechDemo.Shared;
using NextechDemo.Shared.Models;
using NextechDemo.Shared.Models.News;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NextechDemo.Application.Services.NewsProvider
{
    public class NewsProviderService : INewsProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NewsProviderService> _logger;
        private readonly ICacheService _cacheService;

        public NewsProviderService(HttpClient httpClient, ILogger<NewsProviderService> logger, ICacheService cacheService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <summary>
        ///     List of all top news  
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<NewsResponseModel>>> GetNews(PageParams pageParams)
        {
            Response<List<NewsResponseModel>> newsResult = new Response<List<NewsResponseModel>>();
            if (_cacheService.Get<List<int>>(_cacheService.defaultCache()) is null)
            {
                var httpResponse = await _httpClient.GetAsync(ApiConfig.NewsConfig.News());
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var serializationOption = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"[{httpResponse.StatusCode}] An error occurred while requesting news api.");
                    var errorResponse = JsonSerializer.Deserialize<NewsProviderApiException>(jsonString, serializationOption);
                    throw new NewsProviderApiException(errorResponse.Message, errorResponse.Status);
                }
                var newsIds = _cacheService.Set(_cacheService.defaultCache(),
                             JsonSerializer.Deserialize<List<int>>(jsonString, new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = true
                             }));
                newsResult = await PopulateNewsIds(pageParams, newsResult, newsIds);
            }
            else
            {
                var newsIds = _cacheService.Get<List<int>>(_cacheService.defaultCache());
                newsResult = await PopulateNewsIds(pageParams, newsResult, newsIds);
            }
            return newsResult;
        }

        /// <summary>
        ///     Populate news
        /// </summary>
        /// <param name="pageParams"></param>
        /// <param name="newsResult"></param>
        /// <param name="newsIds"></param>
        /// <returns></returns>
        private async Task<Response<List<NewsResponseModel>>> PopulateNewsIds(PageParams pageParams, Response<List<NewsResponseModel>> newsResult, List<int>? newsIds)
        {
            if (newsIds is not null && newsIds.Count > 0)
            {
                newsResult = await GetNewsDetails(newsIds, pageParams);
            }
            return newsResult;
        }

        /// <summary>
        ///     List of all top news details 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<NewsResponseModel>>> GetNewsDetails(List<int> newsIds, PageParams pageParams)
        {
            List<NewsResponseModel> topNews = new List<NewsResponseModel>();
            var cacheKey = _cacheService.PrepareCacheKey("cachekey.datadetailsitems");
            if (_cacheService.Get<NewsModel>(cacheKey) is null)
            {
                var tasks = new List<Task<NewsResponseModel>>();
                int count = 0;
                foreach (var newsId in newsIds)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    if (count > 200)
                    {
                        continue;
                    }
                    async Task<NewsResponseModel> func()
                    {
                        var response = await GetNewsById(newsId);
                        if (!string.IsNullOrEmpty(response.Url))
                        {
                            count++;
                        }
                        return response;
                    }
                    tasks.Add(func());
                }
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    var response = await task;  
                    if (response is not null)
                    {
                        topNews.Add(response);
                    }
                }
                var newsList = _cacheService.Set(cacheKey, new NewsModel {newsList = topNews.Where(x => !string.IsNullOrEmpty(x.Url)).Take(200).ToList()});
                return await ApplyFilter(newsList.newsList, pageParams).ToPageResult(pageParams);
            }
            else
            {
                return await ApplyFilter(_cacheService.Get<NewsModel>(cacheKey).newsList, pageParams).ToPageResult(pageParams);
            }
        }
        /// <summary>
        /// Get details news by news id
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        /// <exception cref="NewsProviderApiException"></exception>
        public async Task<NewsResponseModel?> GetNewsById(int newsId)
        {
            var httpResponse = await _httpClient.GetAsync(ApiConfig.NewsConfig.NewsById(newsId));
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var serializationOption = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"[{httpResponse.StatusCode}] An error occurred while requesting news api.");
                var errorResponse = JsonSerializer.Deserialize<NewsProviderApiException>(jsonString, serializationOption);
                throw new NewsProviderApiException(errorResponse.Message, errorResponse.Status);
            }
            var news = JsonSerializer.Deserialize<NewsResponseModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return news;
        }
        /// <summary>
        /// Apply search
        /// </summary>
        /// <param name="news"></param>
        /// <param name="pageParams"></param>
        /// <returns></returns>
        private List<NewsResponseModel> ApplyFilter(List<NewsResponseModel> news , PageParams pageParams)
        {
            if (!string.IsNullOrEmpty(pageParams.Search))
            {
                return news.Where(x => Regex.IsMatch(x.Title.ToLower().Trim(), @$"\b{pageParams.Search.ToLower().Trim()}\b")).Take(200).ToList();
            }
            return news;
        }
    }
}
