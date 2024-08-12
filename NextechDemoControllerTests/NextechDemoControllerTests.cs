using Microsoft.AspNetCore.Mvc;
using Moq;
using NextechDemo.Api.Controllers;
using NextechDemo.Application.Common.Exception;
using NextechDemo.Application.Services.NewsProvider;
using NextechDemo.Shared;
using NextechDemo.Shared.Models;
using NextechDemo.Shared.Models.News;

namespace NextechDemoControllerTests
{
    public class Tests
    {
        private Mock<INewsProviderService> _mockNewsProviderService;
        private NextechDemoController _controller;

        [SetUp]
        public void Setup()
        {
            _mockNewsProviderService = new Mock<INewsProviderService>();
            _controller = new NextechDemoController(_mockNewsProviderService.Object);
        }

        [Test]
        public async Task GetNewsList_ShouldReturnOkResult_WithNewsData()
        {
            // Arrange
            var query = new PageParams { PageNumber = 1, PageSize = 2, Search = string.Empty, IsFullDataRequired = false };
            var fakeNewsList = new Response<List<NewsResponseModel>>
            {
                PageInfo = new PageInfo
                {
                    CurrentPage = 1,
                    PageSize = 2,
                    TotalPages = 100,
                    TotalCount = 200,
                    IsFullDataRequired = false
                },
                Results = new List<NewsResponseModel>
              {
                   new NewsResponseModel { Title = "News 1", Url = "https://example.com/news/1" },
                   new NewsResponseModel { Title = "News 2", Url = "https://example.com/news/2" }
              },
                IsSuccess = true,
                Message = ""
            };
          
            var response = fakeNewsList;

            _mockNewsProviderService.Setup(x => x.GetNews(query)).ReturnsAsync(response);

            // Act
            var result = await _controller.Post(query);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(200, Is.EqualTo(okResult.StatusCode));

            var returnedResponse = okResult.Value as Response<List<NewsResponseModel>>;
            Assert.IsNotNull(returnedResponse);
            Assert.That(2, Is.EqualTo(returnedResponse.Results.Count));
            Assert.That("News 1", Is.EqualTo(returnedResponse.Results[0].Title));
        }

        [Test]
        public async Task GetNewsList_ShouldReturnBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var pageParams = new PageParams();
           
            _mockNewsProviderService.Setup(x => x.GetNews(It.IsAny<PageParams>()))
                .ThrowsAsync((new NewsProviderApiException("Bad Request",400)));

            // Act && Assert
            Assert.ThrowsAsync<NewsProviderApiException>(() => _controller.Post(pageParams));
        }
    }
}