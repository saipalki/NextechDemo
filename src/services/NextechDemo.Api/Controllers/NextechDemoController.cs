using Microsoft.AspNetCore.Mvc;
using NextechDemo.Api.Validators;
using NextechDemo.Application.Services.NewsProvider;
using NextechDemo.Shared.Models;


namespace NextechDemo.Api.Controllers
{
    /// <summary>
    ///     Nextech api controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NextechDemoController : ControllerBase
    {
        private readonly INewsProviderService _newsProviderService;
        public NextechDemoController(INewsProviderService newsProviderService)
        {
            _newsProviderService = newsProviderService;
        }

        /// <summary>
        ///     Get top news results  
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetNewsList")]
        public async Task<IActionResult> Post(PageParams pageParams)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            NewsRequestModelValidatator validator = new NewsRequestModelValidatator();
            var validationResult = validator.Validate(pageParams);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return Ok(await _newsProviderService.GetNews(pageParams));
        }
    }
}
