using NextechDemo.Shared;
using NextechDemo.Shared.Models;

namespace NextechDemo.Application.Helper
{
    /// <summary>
    ///     Helper class for paging related methods
    /// </summary>
    public static class PageExtension
    {
        /// <summary>
        ///     Extension method to get page info and data from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageParams"></param>
        /// <returns cref = "Response{T}"> Page result set with page info</returns>
        public static async Task<Response<List<T>>> ToPageResult<T>(this List<T> query, PageParams pageParams) where T : class
        {
            // Get page related info
            var pageInfo = new PageInfo
            {
                CurrentPage = pageParams.PageNumber,
                PageSize = pageParams.PageSize,
                IsFullDataRequired = pageParams.IsFullDataRequired,
                TotalCount = query.Count,
            };
            var pageCount = (double)pageInfo.TotalCount / pageParams.PageSize;
            pageInfo.TotalPages = (int)Math.Ceiling(pageCount);
            //Get no of pages to be skipped
            var skip = (pageParams.PageNumber - 1) * pageParams.PageSize;
            return new Response<List<T>>
            {
                IsSuccess = true,
                PageInfo = pageInfo,
                // Query to return page data
                Results = !pageParams.IsFullDataRequired ? query.Skip(skip).Take(pageParams.PageSize).ToList() : query,
            };
        }
    }
}
