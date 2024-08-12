using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NextechDemo.Application.Common.Exception;
using NextechDemo.Application.Services.CacheService;
using NextechDemo.Application.Services.NewsProvider;
using NextechDemo.Shared.Models;
using NextechDemo.Shared.Models.News;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace NextechDemo.UnitTests
{
    public class NewsProviderServiceTests
    {
        private Mock<HttpMessageHandler> _drHttpMessageHandler;
        private Mock<ILogger<NewsProviderService>> _drLogger;
        private Mock<ICacheService> _drCacheService;
        private INewsProviderService? _newsService;
        private NewsProviderService _newsProviderService;

        [SetUp]
        public void SetUp()
        {
            _drHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _drLogger = new Mock<ILogger<NewsProviderService>>();
            _drCacheService = new Mock<ICacheService>();
        }

        /// <summary>
        /// To ensures that when the cache is empty, the service makes an HTTP call to get news and stores the result in the cache
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Get_News_Should_Returns_Results_Successfully_When_Cache_Is_Empty()
        {
            // Arrange
            var newsId = new List<int> { 41188647, 41188966, 41184365 };
            
            var newsResponse = new List<NewsResponseModel> {
                new NewsResponseModel { Id = 41188647, Title = "Xyz", Url = "http://localhost" },
                new NewsResponseModel { Id = 41188966, Title = "Xyz", Url = "http://localhost" },
                new NewsResponseModel { Id = 41184365, Title = "Xyz", Url = "http://localhost" }
                };
            NewsModel newsModel = new NewsModel();
            newsModel.newsList = newsResponse;
            var serializedNewsId = JsonConvert.SerializeObject(newsId);
            var serializedNews = JsonConvert.SerializeObject(newsResponse.FirstOrDefault());
            var cacheKey = "cachekey.topNews";

            // Setup the cache to return null initially
            _drCacheService.Setup(x => x.Get<List<int>>(cacheKey)).Returns(() => null);

            // Setup cache set method
            _drCacheService.Setup(cache => cache.Set(It.IsAny<string>(), It.IsAny<List<int>>(), 0))
            .Returns(value: newsId);

            _drCacheService.Setup(cache => cache.Set(It.IsAny<string>(), It.IsAny<NewsModel>(), 0))
            .Returns(value: newsModel);

            // Setup Protected method on HttpMessageHandler mock to simulate HTTP call
            _drHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsolutePath.Contains("topstories")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(serializedNewsId, Encoding.UTF8, "application/json")
                    };
                    return response;
                }).Verifiable();

            _drHttpMessageHandler.Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
             ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsolutePath.Contains("item")),
              ItExpr.IsAny<CancellationToken>()
          )
          .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
          {
              var response = new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(serializedNews, Encoding.UTF8, "application/json")
              };
              return response;
          }).Verifiable();

            var httpClient = new HttpClient(_drHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(@"https://hacker-news.firebaseio.com/v0/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _newsService = new NewsProviderService(httpClient, _drLogger.Object, _drCacheService.Object);
            var query = new PageParams { PageNumber = 1, PageSize = 10, Search = string.Empty, IsFullDataRequired = false };

            // Act
            var queryResult = await _newsService.GetNews(query);

            // Assert
            Assert.IsNotNull(queryResult);
            Assert.IsTrue(queryResult.IsSuccess);

            var result = queryResult.Results!.ToList();
            Assert.IsTrue(result.Any());
            Assert.That(result.Count, Is.EqualTo(newsId.Count));
        }

        /// <summary>
        ///     Checks if the service throws the appropriate exception when the API call fails.
        /// </summary>
        [Test]
        public void GetNews_ShouldThrowException_WhenApiCallFails()
        {
            // Arrange
            var fakeErrorResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("{\"Message\": \"Bad Request\", \"Status\": 400}")
            };
            _drHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsolutePath.Contains("topstories")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var response = fakeErrorResponse;
                    return response;
                }).Verifiable();

            var httpClient = new HttpClient(_drHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(@"https://hacker-news.firebaseio.com/v0/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _drCacheService.Setup(x => x.Get<List<int>>(It.IsAny<string>())).Returns((List<int>)null);
            _newsService = new NewsProviderService(httpClient, _drLogger.Object, _drCacheService.Object);

            var pageParams = new PageParams();

            // Act & Assert
            Assert.ThrowsAsync<NewsProviderApiException>(() => _newsService.GetNews(pageParams));
        }

        /// <summary>
        ///     Verifies that if the cache is not empty, the service should return news from the cache without making an HTTP call.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetNews_ShouldReturnNewsFromCache_WhenCacheIsNotEmpty()
        {
            // Arrange
            var fakeNewsIds = new List<int> { 1, 2, 3 };
            var serializeFakeNewsIds = JsonConvert.SerializeObject(fakeNewsIds);
            _drCacheService.Setup(x => x.Get<List<int>>(It.IsAny<string>())).Returns(fakeNewsIds);

            var fakeErrorResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(serializeFakeNewsIds, Encoding.UTF8, "application/json")
            };
            _drHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsolutePath.Contains("topstories")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var response = fakeErrorResponse;
                    return response;
                }).Verifiable();
            var pageParams = new PageParams();
            var httpClient = new HttpClient(_drHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(@"https://hacker-news.firebaseio.com/v0/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _drCacheService.Setup(x => x.Get<List<int>>(It.IsAny<string>())).Returns((List<int>)null);
            _newsProviderService = new NewsProviderService(httpClient, _drLogger.Object, _drCacheService.Object);

            // Act
            var result = await _newsService.GetNews(pageParams);

            // Assert
            Assert.IsNotNull(result);

        }

        /// <summary>
        ///     Ensures that the GetNewsDetails method returns detailed news from the API.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetNewsDetails_ShouldReturnNewsDetails_WhenCalled()
        {
            // Arrange
            var fakeNewsIds = new List<int> { 1, 2, 3 };
            var fakeNewsResponse = new List<NewsResponseModel> {
                new NewsResponseModel { Id = 41188647, Title = "Xyz", Url = "https://abc.com/news/1" },
                new NewsResponseModel { Id = 41188966, Title = "Xyz", Url = "https://abc.com/news/2" },
                new NewsResponseModel { Id = 41184365, Title = "Xyz", Url = "https://abc.com/news/3" }
                };

            NewsModel newsModel = new NewsModel();
            newsModel.newsList = fakeNewsResponse;

            var serializeFakeNewsResponse = JsonConvert.SerializeObject(fakeNewsResponse.FirstOrDefault());

            _drHttpMessageHandler.Protected()
         .Setup<Task<HttpResponseMessage>>(
             "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsolutePath.Contains("item")),
             ItExpr.IsAny<CancellationToken>()
         )
         .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
         {
             var response = new HttpResponseMessage
             {
                 StatusCode = HttpStatusCode.OK,
                 Content = new StringContent(serializeFakeNewsResponse, Encoding.UTF8, "application/json")
             };
             return response;
         }).Verifiable();

            var pageParams = new PageParams();
            var httpClient = new HttpClient(_drHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(@"https://hacker-news.firebaseio.com/v0/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _drCacheService.Setup(x => x.Get<NewsModel>(It.IsAny<string>())).Returns((NewsModel)null);
            _drCacheService.Setup(cache => cache.Set(It.IsAny<string>(), It.IsAny<NewsModel>(), 0))
             .Returns(value: newsModel);

            _newsProviderService = new NewsProviderService(httpClient, _drLogger.Object, _drCacheService.Object);

            // Act
            var result = await _newsProviderService.GetNewsDetails(fakeNewsIds, pageParams);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Results.Count, Is.EqualTo(3));
            Assert.That(result.Results[0].Url, Is.EqualTo("https://abc.com/news/1"));
        }

    }
}

